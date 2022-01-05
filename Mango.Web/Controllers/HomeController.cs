using Mango.Web.Models;
using Mango.Web.Services;
using Mango.Web.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto> products = new List<ProductDto>();
        var response = await _productService.GetAllProductsAsync<ResponseDto>();
        if (response is { IsSuccess:true})
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        }
        
        return View(products);
    }
    
    [Authorize]
    public async Task<IActionResult> Details(int productId)
    {
        ProductDto model = new ();
        var response = await _productService.GetProductByIdAsync<ResponseDto>(productId);
        if (response is { IsSuccess:true})
        {
            model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
        }
        
        return View(model);
    }

    [HttpPost]
    //[ActionName("Details")]
    [Authorize]
    public async Task<IActionResult> Details(ProductDto productDto)
    {
        CartDto cartDto = new CartDto
        {
            Header = new CartHeaderDto
            {
                UserId = User.Claims.Where(user => user.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailDto cartDetailDto = new CartDetailDto
        {
            Count = productDto.Count,
            ProductId = productDto.Id
        };

        var response = await _productService.GetProductByIdAsync<ResponseDto>(productDto.Id);
        if (response is { IsSuccess: true })
        {
            cartDetailDto.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
        }
        List<CartDetailDto> cartDetails = new() { cartDetailDto };
        cartDto.Details = cartDetails;

        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var addedToCartResponse = await _cartService.AddToCartAsync<ResponseDto>(cartDto, accessToken);
        if (addedToCartResponse is { IsSuccess: true })
        {
            return RedirectToAction(nameof(Index));
        }

        return View(productDto);
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        var tokenAsync = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}