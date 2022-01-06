namespace Mango.Web.Services.Contracts;

public interface ICouponService
{
    Task<T> GetCoupon<T>(string couponCode, string? token = null);
}