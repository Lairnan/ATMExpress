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
[Route("api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly IRepository<Transaction> _repository;

    public TransactionController(IRepository<Transaction> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetTransactionById(Guid id)
    {
        var transaction = _repository.FindById(id);
        if (transaction == null)
            return NotFound();

        var jsonTransaction = JsonConvert.SerializeObject(transaction);

        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("transaction_found"),
            Data = jsonTransaction
        };
        return Ok(response);
    }
        
    [HttpGet("count")]
    public IActionResult GetTransactionCount()
    {
        var count = _repository.GetCount();
        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("transaction_count"),
            Data = JsonConvert.SerializeObject(count)
        };
        return Ok(response);
    }

    [HttpGet("getall")]
    public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 40)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 40;
        var transactions = _repository.GetAll(page, pageSize);
        var jsonTransactions = JsonConvert.SerializeObject(transactions);

        return Ok(jsonTransactions);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
    {
        try
        {
            await _repository.AddAsync(transaction);
            var jsonTransaction = JsonConvert.SerializeObject(transaction);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("transaction_created"),
                Data = jsonTransaction
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("transaction_creation_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] Transaction transaction)
    {
        try
        {
            transaction.Id = id;
            await _repository.UpdateAsync(transaction);

            var jsonTransaction = JsonConvert.SerializeObject(transaction);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("transaction_updated"),
                Data = jsonTransaction
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("transaction_update_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        try
        {
            var transaction = _repository.FindById(id);
            if (transaction == null)
                return NotFound();

            await _repository.DeleteAsync(transaction);

            var jsonTransaction = JsonConvert.SerializeObject(transaction);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("transaction_deleted"),
                Data = jsonTransaction
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("transaction_deletion_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }
}