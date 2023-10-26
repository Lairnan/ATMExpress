﻿using CSA.DTO.Responses;
using CSA.Entities;
using DatabaseManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AccessHub.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductRepository _repository;

    public ProductController(ProductRepository repository)
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

        var response = new ApiResponse<string>
        {
            Success = true,
            Message = "product_found",
            Data = jsonProduct
        };
        return Ok(response);
    }

    [HttpGet("getall")]
    public IActionResult GetAllProducts()
    {
        var products = _repository.GetAll();
        var jsonProducts = JsonConvert.SerializeObject(products);

        return Ok(jsonProducts);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try
        {
            _repository.Add(product);
            await _repository.SaveAsync();
            var jsonProduct = JsonConvert.SerializeObject(product);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "product_created",
                Data = jsonProduct
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "product_creation_error",
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
            _repository.Update(product);
            await _repository.SaveAsync();

            var jsonProduct = JsonConvert.SerializeObject(product);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "product_updated",
                Data = jsonProduct
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "product_update_error",
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

            _repository.Delete(product);
            await _repository.SaveAsync();

            var jsonProduct = JsonConvert.SerializeObject(product);

            var response = new ApiResponse<string>
            {
                Success = true,
                Message = "product_deleted",
                Data = jsonProduct
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "product_deletion_error",
                Data = ex.Message
            };
            return BadRequest(response);
        }
    }
}