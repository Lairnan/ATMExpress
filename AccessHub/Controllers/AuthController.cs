using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement;
using DatabaseManagement.Repositories;
using IIC.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AccessHub.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IRepository<User> _userRepository;

    public AuthController(DatabaseManagementContext dbContext)
    {
        this._userRepository = new UserRepository(dbContext);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null) return BadRequest(new { message = "Bad request" });
        User user;
        if ((user = _userRepository.GetAll().FirstOrDefault(s => s.Login == request.Login)!) == null)
            return Unauthorized(new { message = "Wrong login" } );
        if (user.Password != request.Password) return Unauthorized(new { message = "Wrong password" } );
        
        var loginResponse = JsonSerializer.Serialize(new LoginResponse
        {
            Token = GetToken(),
            UserId = user.Id
        });
        
        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "Успешный вход",
            Data = loginResponse
        };
        return Ok(response);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (request == null) return BadRequest(new { message = "Bad request" });
        if (string.IsNullOrWhiteSpace(request.Login)) return BadRequest(new { message = "Login can't be empty" });
        if (string.IsNullOrWhiteSpace(request.Password)) return BadRequest(new { message = "Password can't be empty" });
        if (_userRepository.GetAll().FirstOrDefault(s => s.Login == request.Login) != null) return BadRequest(new { message = "User login already exists" });

        var user = new User
        {
            Login = request.Login,
            Password = request.Password
        };
        
        _userRepository.Add(user);
        _userRepository.Save();
        
        var loginResponse = JsonSerializer.Serialize(new LoginResponse
        {
            Token = GetToken(),
            UserId = user.Id
        });
        
        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "Успешная регистрация",
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