using Mango.Web.Models;
using Mango.Web.Services.Contracts;
using static Mango.Web.StaticDetails;

namespace Mango.Web.Services;

public class CouponService:BaseService, ICouponService
{
    public CouponService(IHttpClientFactory httpClient) : base(httpClient)
    {
    }

    public async Task<T> GetCoupon<T>(string couponCode, string? token)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = ApiType.GET,
            Url = $"{CouponApiBase}/api/coupons/{couponCode}",
            AccessToken = token
        });
    }
}