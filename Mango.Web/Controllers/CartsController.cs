using Mango.Web.Models;
using Mango.Web.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers;

public class CartsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public CartsController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await LoadCartBasedOnLoggedInUser());
    }

    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

        if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));

        return View();
    }

    private async Task<CartDto> LoadCartBasedOnLoggedInUser()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

        CartDto cartDto = new();
        if (response is { IsSuccess: true })
            cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));

        if (cartDto.Header != null)
            foreach (var cartDtoDetail in cartDto.Details)
                cartDto.Header.OrderTotal += cartDtoDetail.Product.Price * cartDtoDetail.Count;

        return cartDto;
    }
}