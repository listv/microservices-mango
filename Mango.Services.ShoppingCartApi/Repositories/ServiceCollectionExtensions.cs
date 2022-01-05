using Mango.Services.ShoppingCartApi.Repositories.Contracts;

namespace Mango.Services.ShoppingCartApi.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICartRepository, CartRepository>();

        return services;
    }
}