using Mango.MessageBus;
using Mango.Services.ShoppingCartApi.Repositories;
using Mango.Services.ShoppingCartApi.Repositories.Contracts;

namespace Mango.Services.ShoppingCartApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICouponRepository, CouponRepository>();

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

        return services;
    }

    public static IServiceCollection RegisterHttpClients(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        services.AddHttpClient<ICouponRepository, CouponRepository>(client =>
            client.BaseAddress = new Uri(configuration["ServiceUrls:CouponApi"]));

        return services;
    }
}