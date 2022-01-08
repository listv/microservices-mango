using Mango.Services.OrderApi.Models;

namespace Mango.Services.OrderApi.Repositories.Contracts;

public interface IOrderRepository
{
    Task<bool> AddOrder(OrderHeader orderHeader);
    Task UpdateOrderPaymentStatus(int orderHeaderId, bool isPaid);
}