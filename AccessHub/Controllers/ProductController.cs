using Configuration;
using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccessHub.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IRepository<Product> _repository;

    public ProductController(IRepository<Product> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetProductById(Guid id)
    {
        var product = _repository.FindById(id);
        if (product == null)
            return NotFound();

        var jsonProduct = JsonConvert.SerializeObject(product);

        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("product_found"),
            Data = jsonProduct
        };
        return Ok(response);
    }
    
    [HttpGet("count")]
    public IActionResult GetProductCount()
    {
        var count = _repository.GetCount();
        var response = new ApiResponse
        {
            Success = true,
            Message = Translate.GetString("product_count"),
            Data = JsonConvert.SerializeObject(count)
        };
        return Ok(response);
    }

    [HttpGet("getall")]
    public IActionResult GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 40)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 40;
        var products = _repository.GetAll(page, pageSize);
        var jsonProducts = JsonConvert.SerializeObject(products);

        return Ok(jsonProducts);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try
        {
            await _repository.AddAsync(product);
            var jsonProduct = JsonConvert.SerializeObject(product);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("product_created"),
                Data = jsonProduct
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("product_creation_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
    {
        try
        {
            product.Id = id;
            await _repository.UpdateAsync(product);

            var jsonProduct = JsonConvert.SerializeObject(product);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("product_updated"),
                Data = jsonProduct
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("product_update_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        try
        {
            var product = _repository.FindById(id);
            if (product == null)
                return NotFound();

            await _repository.DeleteAsync(product);

            var jsonProduct = JsonConvert.SerializeObject(product);

            var response = new ApiResponse
            {
                Success = true,
                Message = Translate.GetString("product_deleted"),
                Data = jsonProduct
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = Translate.GetString("product_deletion_error"),
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }
}