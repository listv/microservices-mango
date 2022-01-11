namespace Mango.Services.PaymentApi.Messaging.Contracts;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}