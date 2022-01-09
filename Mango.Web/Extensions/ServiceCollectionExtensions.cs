using Mango.Web.Services;
using Mango.Web.Services.Contracts;

namespace Mango.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IProductService, ProductService>()
            .AddScoped<ICartService, CartService>()
            .AddScoped<ICouponService, CouponService>();
    }

    public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IProductService, ProductService>();
        services.AddHttpClient<ICartService, CartService>();
        services.AddHttpClient<ICouponService, CouponService>();

        return services;
    }
}