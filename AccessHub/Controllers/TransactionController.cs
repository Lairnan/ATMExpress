using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccessHub.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionRepository _repository;

        public TransactionController(TransactionRepository repository)
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

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "transaction_found",
                Data = jsonTransaction
            };
            return Ok(response);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var transactions = _repository.GetAll();
            var jsonTransactions = JsonConvert.SerializeObject(transactions);

            return Ok(jsonTransactions);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                _repository.Add(transaction);
                await _repository.SaveAsync();
                var jsonTransaction = JsonConvert.SerializeObject(transaction);

                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "transaction_created",
                    Data = jsonTransaction
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "transaction_creation_error",
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
                _repository.Update(transaction);
                await _repository.SaveAsync();

                var jsonTransaction = JsonConvert.SerializeObject(transaction);

                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "transaction_updated",
                    Data = jsonTransaction
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "transaction_update_error",
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

                _repository.Delete(transaction);
                await _repository.SaveAsync();

                var jsonTransaction = JsonConvert.SerializeObject(transaction);

                var response = new ApiResponse<string>
                {
                    Success = true,
                    Message = "transaction_deleted",
                    Data = jsonTransaction
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "transaction_deletion_error",
                Data = ex.Message
                };
                return BadRequest(response);
            }
        }
    }
}
