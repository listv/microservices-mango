namespace Mango.Services.ShoppingCartApi.Models;

public class CartDetail
{
    public int Id { get; set; }
    
    public int CartHeaderId { get; set; }
    public CartHeader CartHeader { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Count { get; set; }
}