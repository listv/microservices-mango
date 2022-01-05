namespace Mango.Services.ShoppingCartApi.Models;

public class Cart
{
    public CartHeader Header { get; set; } = null!;
    public IEnumerable<CartDetail> Details { get; set; } = null!;
}