using Mango.Services.ShoppingCartApi.Models.Dto;

namespace Mango.Services.ShoppingCartApi.Repositories.Contracts;

public interface ICouponRepository
{
    Task<CouponDto> GetCoupon(string couponName);
}