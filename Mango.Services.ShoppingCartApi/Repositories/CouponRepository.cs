using Mango.Services.ShoppingCartApi.Models.Dto;
using Mango.Services.ShoppingCartApi.Repositories.Contracts;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartApi.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _client;

    public CouponRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<CouponDto> GetCoupon(string couponName)
    {
        var response = await _client.GetAsync($"/api/coupons/{couponName}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if (responseDto is { IsSuccess: true })
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));

        return new CouponDto();
    }
}