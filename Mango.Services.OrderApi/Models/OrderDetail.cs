namespace Mango.Services.OrderApi.Models;

public class OrderDetail
{
    public int Id { get; set; }
    
    public int OrderHeaderId { get; set; }
    public OrderHeader OrderHeader { get; set; } = null!;

    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public double Price { get; set; }
    public int Count { get; set; }
}