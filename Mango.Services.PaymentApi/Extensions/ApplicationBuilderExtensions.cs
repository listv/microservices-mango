using Mango.Services.PaymentApi.Messaging.Contracts;

namespace Mango.Services.PaymentApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; } = null!;

    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    {
        ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
        var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

        hostApplicationLife?.ApplicationStarted.Register(OnStart);
        hostApplicationLife?.ApplicationStopped.Register(OnStop);

        return app;
    }

    private static void OnStart()
    {
        ServiceBusConsumer.Start();
    }

    private static void OnStop()
    {
        ServiceBusConsumer.Stop();
    }
}