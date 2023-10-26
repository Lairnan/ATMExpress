using System.IdentityModel.Tokens.Jwt;
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
        
        var loginResponse = JsonConvert.SerializeObject(new LoginResponse
        {
            Token = GetToken(),
            UserId = user.Id
        });
        
        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "user_login_successful",
            Data = loginResponse
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

        var loginResponse = JsonConvert.SerializeObject(new LoginResponse
        {
            Token = GetToken(),
            UserId = user.Id
        });
        
        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "user_registration_successful",
            Data = loginResponse
        };
        return Ok(response);
    }

    private static string GetToken()
    {
        var jwt = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey,
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}