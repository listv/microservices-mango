using Mango.Services.ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Mango.Services.ShoppingCartApi.DbContexts;

public class ApplicationDbContext:DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<CartHeader> CartHeaders { get; set; } = null!;
    public DbSet<CartDetail> CartDetails { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}