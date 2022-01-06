using Mango.Services.CouponApi.Repositories.Contracts;

namespace Mango.Services.CouponApi.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ICouponRepository, CouponRepository>();
    }
}