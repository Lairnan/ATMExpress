using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccessHub.Controllers;

[ApiController]
[Route("api/cards")]
public class CardController : ControllerBase
{
    private readonly CardRepository _repository;

    public CardController(CardRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetCardById(Guid id)
    {
        var card = _repository.FindById(id);
        if (card == null)
            return NotFound();

        var jsonCard = JsonConvert.SerializeObject(card);

        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "card_founded",
            Data = jsonCard
        };
        return Ok(response);
    }

    [HttpGet("getall")]
    public IActionResult GetAllCards()
    {
        var cards = _repository.GetAll();
        var jsonCards = JsonConvert.SerializeObject(cards);

        return Ok(jsonCards);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCard([FromBody] Card card)
    {
        try
        {
            _repository.Add(card);
            await _repository.SaveAsync();
            var jsonCard = JsonConvert.SerializeObject(card);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "card_created",
                Data = jsonCard
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "card_creation_error",
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateCard(Guid id, [FromBody] Card card)
    {
        try
        {
            card.Id = id;
            _repository.Update(card);
            await _repository.SaveAsync();

            var jsonCard = JsonConvert.SerializeObject(card);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "card_updated",
                Data = jsonCard
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "card_update_error",
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteCard(Guid id)
    {
        try
        {
            var card = _repository.FindById(id);
            if (card == null)
                return NotFound();

            _repository.Delete(card);
            await _repository.SaveAsync();

            var jsonCard = JsonConvert.SerializeObject(card);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "card_deleted",
                Data = jsonCard
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "card_deletion_error",
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }
}