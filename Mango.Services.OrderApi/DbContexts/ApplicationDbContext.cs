using Mango.Services.OrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderApi.DbContexts;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
    }

    public DbSet<OrderHeader> OrderHeaders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
}