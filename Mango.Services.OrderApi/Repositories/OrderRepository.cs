using Mango.Services.OrderApi.DbContexts;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderApi.Repositories;

public class OrderRepository:IOrderRepository
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContext;

    public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddOrder(OrderHeader orderHeader)
    {
        try
        {
            await using var db = new ApplicationDbContext(_dbContext);
            db.OrderHeaders.Add(orderHeader);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool isPaid)
    {
        await using (var db = new ApplicationDbContext(_dbContext))
        {
            var orderHeaderFromDb = await db.OrderHeaders.FirstOrDefaultAsync(header => header.Id == orderHeaderId);
            if (orderHeaderFromDb!=null)
            {
                orderHeaderFromDb.PaymentStatus = isPaid;
                await db.SaveChangesAsync();
            }
        }
    }
}