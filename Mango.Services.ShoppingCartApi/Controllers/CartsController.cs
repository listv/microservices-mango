using Mango.Services.ShoppingCartApi.Models.Dto;
using Mango.Services.ShoppingCartApi.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartApi.Controllers;

[ApiController]
[Route("api/carts")]
public class CartsController : Controller
{
    private readonly ICartRepository _cartRepository;
    protected ResponseDto _response;

    public CartsController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
        _response = new ResponseDto();
    }

    [HttpGet("GetCart/{userId}")]
    public async Task<object> Get(string userId)
    {
        try
        {
            CartDto cartDto = await _cartRepository.GetCartByUserId(userId);
            _response.Result = cartDto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpPost("AddCart")]
    public async Task<object> AddCart(CartDto cartDto)
    {
        try
        {
            CartDto cart = await _cartRepository.CreateUpdateCart(cartDto);
            _response.Result = cart;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpPut("UpdateCart")]
    public async Task<object> Update(CartDto cartDto)
    {
        try
        {
            CartDto cart = await _cartRepository.CreateUpdateCart(cartDto);
            _response.Result = cart;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }

    [HttpDelete("RemoveCart/{id}")]
    public async Task<object> Remove([FromRoute(Name ="id")]int cartId)
    {
        try
        {
            bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }
    
    [HttpPost("ApplyCoupon")]
    public async Task<object> ApplyCoupon([FromBody]CartDto cartDto)
    {
        try
        {
            bool isSuccess = await _cartRepository.ApplyCoupon(cartDto.Header.UserId, cartDto.Header.CouponCode);
            _response.Result = isSuccess;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }
    
    [HttpPost("RemoveCoupon")]
    public async Task<object> RemoveCoupon([FromBody]string userId)
    {
        try
        {
            bool isSuccess = await _cartRepository.RemoveCoupon(userId);
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