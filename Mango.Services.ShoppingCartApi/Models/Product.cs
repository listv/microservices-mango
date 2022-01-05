namespace Mango.Services.ShoppingCartApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? CategoryName { get; set; }
}