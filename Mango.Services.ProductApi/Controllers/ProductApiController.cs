using Mango.Services.ProductApi.Models.Dtos;
using Mango.Services.ProductApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductApi.Controllers;

[Route("api/products")]
public class ProductApiController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ResponseDto _response;

    public ProductApiController(IProductRepository repository)
    {
        _repository = repository;
        _response = new ResponseDto();
    }

    [HttpGet]
    public async Task<object> Get()
    {
        try
        {
            var productDtos = await _repository.GetProducts();
            _response.Result = productDtos;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<object> Get(int id)
    {
        try
        {
            var productDto = await _repository.GetProductById(id);
            _response.Result = productDto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpPost]
    [Authorize]
    public async Task<object> Post([FromBody] ProductDto productDto)
    {
        try
        {
            var model = await _repository.CreateUpdateProduct(productDto);
            _response.Result = model;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpPut]
    [Authorize]
    public async Task<object> Put([FromBody] ProductDto productDto)
    {
        try
        {
            var model = await _repository.CreateUpdateProduct(productDto);
            _response.Result = model;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<object> Delete(int id)
    {
        try
        {
            var isSuccess = await _repository.DeleteProduct(id);
            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }
}