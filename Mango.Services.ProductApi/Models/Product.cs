namespace Mango.Services.ProductApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}