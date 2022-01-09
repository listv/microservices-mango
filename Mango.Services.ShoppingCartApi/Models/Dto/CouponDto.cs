namespace Mango.Services.ShoppingCartApi.Models.Dto;

public class CouponDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public double DiscountAmount { get; set; }
}