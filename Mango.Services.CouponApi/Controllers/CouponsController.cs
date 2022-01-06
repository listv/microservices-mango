using Mango.Services.CouponApi.Models.Dto;
using Mango.Services.CouponApi.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponApi.Controllers;

[ApiController]
[Route("api/coupons")]
public class CouponsController : Controller
{
    private readonly ICouponRepository _couponRepository;
    protected ResponseDto _response;

    public CouponsController(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
        _response = new ResponseDto();
    }

    [HttpGet("{code}")]
    public async Task<object> GetDiscountForCode(string code)
    {
        try
        {
            var couponDto = await _couponRepository.GetCouponByCode(code);
            _response.Result = couponDto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return _response;
    }
}