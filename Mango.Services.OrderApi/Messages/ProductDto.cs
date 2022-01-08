namespace Mango.Services.OrderApi.Messages;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? CategoryName { get; set; }
    public int Count{ get; set; }
}