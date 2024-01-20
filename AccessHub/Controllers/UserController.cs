using Configuration;
using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccessHub.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IRepository<User> _repository;

    public UserController(IRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUserById(Guid id)
    {
        var user = _repository.FindById(id);
        if (user == null)
            return NotFound();

        var jsonUser = JsonConvert.SerializeObject(user);

        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("user_found"),
            Data = jsonUser
        };
        return Ok(response);
    }
    
    [HttpGet("count")]
    public IActionResult GetUserCount()
    {
        var count = _repository.GetCount();
        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("user_count"),
            Data = JsonConvert.SerializeObject(count)
        };
        return Ok(response);
    }

    [HttpGet("getall")]
    public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 40)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 40;
        var users = _repository.GetAll(page, pageSize);
        var jsonUsers = JsonConvert.SerializeObject(users);

        return Ok(jsonUsers);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        try
        {
            await _repository.AddAsync(user);
            var jsonUser = JsonConvert.SerializeObject(user);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("user_created"),
                Data = jsonUser
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("user_creation_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
    {
        try
        {
            user.Id = id;
            await _repository.UpdateAsync(user);

            var jsonUser = JsonConvert.SerializeObject(user);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("user_updated"),
                Data = jsonUser
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("user_update_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var user = _repository.FindById(id);
            if (user == null)
                return NotFound();

            await _repository.DeleteAsync(user);

            var jsonUser = JsonConvert.SerializeObject(user);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("user_deleted"),
                Data = jsonUser
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("user_deletion_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }
}