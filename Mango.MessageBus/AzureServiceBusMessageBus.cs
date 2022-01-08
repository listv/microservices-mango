using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Mango.MessageBus;

public class AzureServiceBusMessageBus : IMessageBus
{
    // can be improved
    private readonly string connectionString =
        "Endpoint=sb://okmangorestaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=doVgca6vZMuW3qX/aeyypKsh2yS+agnDWMA3ElGss68=";

    public async Task PublishMessage(BaseMessage message, string topicName)
    {
        await using var serviceBusClient = new ServiceBusClient(connectionString);
        var sender = serviceBusClient.CreateSender(topicName);
        
        var jsonMessage = JsonConvert.SerializeObject(message);
        var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString() 
            
        };
        
        await sender.SendMessageAsync(finalMessage);
        await sender.CloseAsync();
    }
}