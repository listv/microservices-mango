using Mango.Services.CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponApi.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            Id = 1,
            Code = "10OFF",
            DiscountAmount = 10
        });
        modelBuilder.Entity<Coupon>().HasData(new Coupon
        {
            Id = 2,
            Code = "20OFF",
            DiscountAmount = 20
        });
    }
}