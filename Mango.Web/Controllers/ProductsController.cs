using Mango.Web.Models;
using Mango.Web.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto> productDtos = new();
        var accessToken = await GetTokenAsync();
        var response = await _productService.GetAllProductsAsync<ResponseDto>(accessToken);
        if (response is { IsSuccess: true })
            productDtos = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        return View(productDtos);
    }

    private async Task<string?> GetTokenAsync()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await GetTokenAsync();
            var response = await _productService.CreateProductAsync<ResponseDto>(model, accessToken);
            if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int productId)
    {
        var accessToken = await GetTokenAsync();
        var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);
        if (response is { IsSuccess: true })
        {
            var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await GetTokenAsync();
            var response = await _productService.UpdateProductAsync<ResponseDto>(model, accessToken);
            if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int productId)
    {
        var accessToken = await GetTokenAsync();
        var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);
        if (response is { IsSuccess: true })
        {
            var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductDto model)
    {
        var accessToken = await GetTokenAsync();
        var response = await _productService.DeleteProductAsync<ResponseDto>(model.Id, accessToken);
        if (response is { IsSuccess: true }) return RedirectToAction(nameof(Index));

        return View(model);
    }
}