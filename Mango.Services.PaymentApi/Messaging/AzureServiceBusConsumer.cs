using Azure.Messaging.ServiceBus;
using Mango.Integration.Payments.PaymentProcessor;
using Mango.MessageBus;
using Mango.Services.PaymentApi.Messages;
using Mango.Services.PaymentApi.Messaging.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.PaymentApi.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly string _orderPaymentResultUpdateTopic;

    private readonly IMessageBus _messageBus;
    private readonly IProcessPayment _processPayment;
    private readonly ServiceBusProcessor _orderPaymentProcessor;

    public AzureServiceBusConsumer(IConfiguration configuration, IMessageBus messageBus, IProcessPayment processPayment)
    {
        _messageBus = messageBus;
        _processPayment = processPayment;

        var serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
        var orderPaymentProcessTopic = configuration.GetValue<string>("OrderPaymentProcessTopic");
        var paymentMessageSubscription = configuration.GetValue<string>("OrderPaymentProcessSubscription");
        _orderPaymentResultUpdateTopic = configuration.GetValue<string>("OrderPaymentResultUpdateTopic");

        var client = new ServiceBusClient(serviceBusConnectionString);
        _orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessTopic, paymentMessageSubscription);
    }

    public async Task Start()
    {
        _orderPaymentProcessor.ProcessMessageAsync += OnProcessOrderPaymentMessageReceived;
        _orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
        await _orderPaymentProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _orderPaymentProcessor.StopProcessingAsync();
        await _orderPaymentProcessor.DisposeAsync();
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task OnProcessOrderPaymentMessageReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        var paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body)!;

        var result = _processPayment.PaymentProcessor();

        var updatePaymentResultMessage = new UpdatePaymentResultMessage
        {
            Status = result,
            OrderId = paymentRequestMessage.OrderId
        };

        try
        {
            await _messageBus.PublishMessage(updatePaymentResultMessage, _orderPaymentResultUpdateTopic);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}