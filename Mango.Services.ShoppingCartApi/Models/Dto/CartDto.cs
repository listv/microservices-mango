namespace Mango.Services.ShoppingCartApi.Models.Dto;

public class CartDto
{
    public CartHeaderDto Header { get; set; } = null!;
    public IEnumerable<CartDetailDto> Details { get; set; } = null!;
}