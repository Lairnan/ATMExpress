using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccessHub.BehaviorsFiles;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AccessHub.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;

    public AuthController(DatabaseManagementContext dbContext)
    {
        _userRepository = new UserRepository(dbContext);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null) return BadRequest(new { message = "bad_request" });
        User user;
        if ((user = _userRepository.GetAll().FirstOrDefault(s => s.Login == request.Login)!) == null)
            return Unauthorized(new { message = "wrong_login" } );
        if (user.Password != request.Password) return Unauthorized(new { message = "wrong_password" } );

        var loginResponse = new LoginResponse
        {
            Token = GetToken(user.Login),
            UserId = user.Id
        };
        if (!LogonHelper.InvalidTokens.TryAdd(loginResponse, true))
            return BadRequest("user_already_logon");
        
        var jsonStr = JsonConvert.SerializeObject(loginResponse);

        var response = new ApiResponse
        {
            Success = true,
            Message = "user_login_successful",
            Data = jsonStr
        };
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null) return BadRequest(new { message = "bad_request" });
        if (string.IsNullOrWhiteSpace(request.Login)) return BadRequest(new { message = "login_empty" });
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest(new { message = "password_empty" });
        if (_userRepository.GetAll().FirstOrDefault(s => s.Login == request.Login) != null) return BadRequest(new { message = "login_exists" });

        var user = new User
        {
            Login = request.Login,
            Password = request.Password
        };
        IActionResult badRequest;
        if ((badRequest = await TryAddUser(user)) != Ok()) return badRequest;
        
        var response = new ApiResponse
        {
            Success = true,
            Message = "user_registration_successful",
            Data = ""
        };
        return Ok(response);
    }

    private async Task<IActionResult> TryAddUser(User user)
    {
        try
        {
            _userRepository.Add(user);
            await _userRepository.SaveAsync();
        }
        catch (Exception ex) when (ex is ArgumentNullException
                                       or ArgumentException
                                       or InvalidOperationException)
        {
            return BadRequest(new { message = ex.Message });
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout(Guid userId)
    {
        var token = this.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var loginResponse = new LoginResponse
        {
            Token = token,
            UserId = userId
        };
        if (LogonHelper.InvalidTokens.TryGetValue(loginResponse, out _)) LogonHelper.InvalidTokens[loginResponse] = false;
        else
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "unable_logout",
                Data = ""
            });

        return Ok(new ApiResponse
        {
            Success = true,
            Message = ""
        });
    }

    private static string GetToken(string login)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Program.Configuration["AppSettings:secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: Program.Configuration["AppSettings:issuer"],
            audience: Program.Configuration["AppSettings:audience"],
            claims: new[] { new Claim(ClaimTypes.Name, login) },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}