using System.Text.Json;
using System.Text.Json.Serialization;
using CSA.Entities;
using DatabaseManagement;
using DatabaseManagement.Repositories;
using IIC.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessHub.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IRepository<User> _repository;

    public UserController(DatabaseManagementContext dbContext)
    {
        _repository = new UserRepository(dbContext);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUserById(Guid id)
    {
        var user = _repository.FindById(id);
        if (user == null) return NotFound();

        var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve };
        return Ok(JsonSerializer.Serialize(user, options));
    }
}