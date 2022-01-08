namespace Mango.Services.OrderApi.Messaging.Contracts;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}