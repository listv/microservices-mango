namespace Mango.Services.Email.Messaging.Contracts;

public interface IAzureServiceBusConsumer
{
    Task Start();
    Task Stop();
}