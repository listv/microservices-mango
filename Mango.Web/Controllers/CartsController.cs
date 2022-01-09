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
    private readonly ICouponService _couponService;

    public CartsController(IProductService productService, ICartService cartService, ICouponService couponService)
    {
        _productService = productService;
        _cartService = cartService;
        _couponService = couponService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await LoadCartBasedOnLoggedInUser());
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
        {
            if (!string.IsNullOrWhiteSpace(cartDto.Header.CouponCode))
            {
                var couponFromApi = await _couponService.GetCoupon<ResponseDto>(cartDto.Header.CouponCode, accessToken);
                if (response is { IsSuccess: true })
                {
                    var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(couponFromApi.Result));
                    cartDto.Header.DiscountTotal = coupon.DiscountAmount;
                }
            }

            foreach (var cartDtoDetail in cartDto.Details)
                cartDto.Header.OrderTotal += cartDtoDetail.Product.Price * cartDtoDetail.Count;

            cartDto.Header.OrderTotal -= cartDto.Header.DiscountTotal;
        }

        return cartDto;
    }

    public async Task<IActionResult> Checkout()
    {
        return View(await LoadCartBasedOnLoggedInUser());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartDto cartDto)
    {
        try
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.Checkout<ResponseDto>(cartDto.Header, accessToken);
            if (response is not { IsSuccess: true })
            {
                TempData["Error"] = response.DisplayMessage;
                return RedirectToAction(nameof(Checkout));
            }

            return RedirectToAction(nameof(Confirmation));
        }
        catch (Exception ex)
        {
            return View(cartDto);
        }
    }

    public Task<IActionResult> Confirmation()
    {
        return Task.FromResult<IActionResult>(View());
    }

    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

        if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.ApplyCouponAsync<ResponseDto>(cartDto, accessToken);

        if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _cartService.RemoveCouponAsync<ResponseDto>(cartDto.Header.UserId, accessToken);

        if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));

        return View();
    }
}