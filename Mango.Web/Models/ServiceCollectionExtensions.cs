using Mango.Web.Services;
using Mango.Web.Services.Contracts;

namespace Mango.Web.Models
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ICartService, CartService>();
        }

        public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IProductService, ProductService>();
            services.AddHttpClient<ICartService, CartService>();

            return services;
        }
    }
}
