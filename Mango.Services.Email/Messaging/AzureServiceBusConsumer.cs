using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Messaging.Contracts;
using Mango.Services.Email.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly IConfiguration _configuration;

    private readonly EmailRepository _emailRepository;
    private readonly string _emailSubscription;
    private readonly string _orderPaymentResultUpdateTopic;
    private readonly string _serviceBusConnectionString;

    private readonly ServiceBusProcessor _orderPaymentResultUpdateProcessor;

    public AzureServiceBusConsumer(EmailRepository emailRepository, IConfiguration configuration)
    {
        _emailRepository = emailRepository;
        _configuration = configuration;

        _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
        _emailSubscription = _configuration.GetValue<string>("EmailSubscription");
        _orderPaymentResultUpdateTopic = _configuration.GetValue<string>("OrderPaymentResultUpdateTopic");

        var serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);

        _orderPaymentResultUpdateProcessor =
            serviceBusClient.CreateProcessor(_orderPaymentResultUpdateTopic, _emailSubscription);
    }

    public async Task Start()
    {
        _orderPaymentResultUpdateProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
        _orderPaymentResultUpdateProcessor.ProcessErrorAsync += ErrorHandler;

        await _orderPaymentResultUpdateProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _orderPaymentResultUpdateProcessor.StartProcessingAsync();
        await _orderPaymentResultUpdateProcessor.DisposeAsync();
    }

    private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
    {
        var argsMessage = args.Message;
        var body = Encoding.UTF8.GetString(argsMessage.Body);

        var updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

        try
        {
            await _emailRepository.SendAndLogEmail(updatePaymentResultMessage);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}