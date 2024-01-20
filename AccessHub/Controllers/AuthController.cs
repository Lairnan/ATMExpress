using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccessHub.BehaviorsFiles;
using AccessHub.Models;
using Configuration;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AccessHub.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IRepository<User> _userRepository;

    public AuthController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null) return BadRequest(new { message = Translate.GetString("null_request") });
        User user;
        if ((user = _userRepository.GetAll().FirstOrDefault(s => s.Login == request.Login)!) == null)
            return Unauthorized(new { message = Translate.GetString("wrong_login") } );
        if (user.Password != request.Password) return Unauthorized(new { message = Translate.GetString("wrong_password") } );

        var loginResponse = new LoginResponse
        {
            Token = GetToken(user.Login),
            UserId = user.Id
        };

        var userToken = LogonHelper.GetUserAuthorize(new UserToken(loginResponse.Token, loginResponse.UserId));
        
        switch (userToken)
        {
            case { Valid: false }:
                userToken.Valid = true;
                break;
            /*case { Valid: true }:
                return BadRequest(Translate.GetString("user_already_logon");*/
            default:
                LogonHelper.UsersToken.Add(new UserToken(true, loginResponse.Token, loginResponse.UserId));
                break;
        }

        if (userToken != null) loginResponse.Token = userToken.Token!;
        var jsonStr = JsonConvert.SerializeObject(loginResponse);

        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("user_login_successful"),
            Data = jsonStr
        };
        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request == null) return BadRequest(new { message = Translate.GetString("bad_request") });
        if (string.IsNullOrWhiteSpace(request.Login)) return BadRequest(new { message = Translate.GetString("login_empty") });
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest(new { message = Translate.GetString("password_empty") });
        if (_userRepository.GetAll().FirstOrDefault(s => s.Login == request.Login) != null) return BadRequest(new { message = Translate.GetString("login_exists") });

        var user = new User
        {
            Login = request.Login,
            Password = request.Password
        };
        
        if (await TryAddUser(user) is BadRequestResult badRequest) return badRequest;
        
        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("user_registration_successful"),
            Data = ""
        };
        return Ok(response);
    }

    private async Task<IActionResult> TryAddUser(User user)
    {
        try
        {
            await _userRepository.AddAsync(user);
        }
        catch (Exception ex) when (ex is ArgumentNullException
                                       or ArgumentException
                                       or InvalidOperationException)
        {
            return BadRequest(new { message = ex.Message });
        }

        return Ok();
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout([FromBody] Guid userId)
    {
        var token = this.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        var userToken = LogonHelper.GetUserAuthorize(new UserToken(token, userId));
        
        if (userToken is { Valid: true }) userToken.Valid = false;
        else return BadRequest(new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("unable_logout"),
                Data = ""
            });

        return Ok(new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("success_logout")
        });
    }

    [HttpGet("get_logon_tokens")]
    public IActionResult GetLogonTokens(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret) || secret != Program.Configuration["AppSettings:secret"])
            return Forbid();
        
        return Ok(JsonConvert.SerializeObject(LogonHelper.UsersToken));
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