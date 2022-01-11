using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.OrderApi.Messages;
using Mango.Services.OrderApi.Messaging.Contracts;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderApi.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly string _orderPaymentProcessTopic;
    private readonly string _orderPaymentResultUpdateTopic;

    private readonly OrderRepository _orderRepository;
    private readonly IMessageBus _messageBus;

    private readonly ServiceBusProcessor _checkoutProcessor;
    private readonly ServiceBusProcessor _orderUpdatePaymentStatusProcessor;

    public AzureServiceBusConsumer(OrderRepository orderRepository,
        //IMapper mapper,
        IConfiguration configuration, IMessageBus messageBus)
    {
        _orderRepository = orderRepository;
        _messageBus = messageBus;
        //_mapper = mapper;

        var serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
        var checkoutMessageTopic = configuration.GetValue<string>("CheckoutMessageTopic");
        _orderPaymentProcessTopic = configuration.GetValue<string>("OrderPaymentProcessTopic");
        _orderPaymentResultUpdateTopic = configuration.GetValue<string>("OrderPaymentResultUpdateTopic");
        var checkoutMessageSubscription = configuration.GetValue<string>("CheckoutMessageSubscription");

        var client = new ServiceBusClient(serviceBusConnectionString);
        _checkoutProcessor = client.CreateProcessor(checkoutMessageTopic, checkoutMessageSubscription);
        _orderUpdatePaymentStatusProcessor =
            client.CreateProcessor(_orderPaymentResultUpdateTopic, checkoutMessageSubscription);
    }

    public async Task Start()
    {
        _checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
        _checkoutProcessor.ProcessErrorAsync += ErrorHandler;
        await _checkoutProcessor.StartProcessingAsync();

        _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
        _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
        await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
    }

    private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);
        var paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

        await _orderRepository.UpdateOrderPaymentStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);
        await args.CompleteMessageAsync(args.Message);
    }

    public async Task Stop()
    {
        await _checkoutProcessor.StopProcessingAsync();
        await _checkoutProcessor.DisposeAsync();

        await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
        await _orderUpdatePaymentStatusProcessor.DisposeAsync();
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        var checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body)!;

        // var orderHeader = _mapper.Map<CheckoutHeaderDto, OrderHeader>(checkoutHeaderDto);
        OrderHeader orderHeader = new()
        {
            UserId = checkoutHeaderDto.UserId,
            FirstName = checkoutHeaderDto.FirstName,
            LastName = checkoutHeaderDto.LastName,
            OrderDetails = new List<OrderDetail>(),
            CardNumber = checkoutHeaderDto.CardNumber,
            CouponCode = checkoutHeaderDto.CouponCode,
            CVV = checkoutHeaderDto.CVV,
            DiscountTotal = checkoutHeaderDto.DiscountTotal,
            Email = checkoutHeaderDto.Email,
            ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
            OrderDateTime = DateTime.Now,
            OrderTotal = checkoutHeaderDto.OrderTotal,
            PaymentStatus = false,
            Phone = checkoutHeaderDto.Phone,
            PickupDateTime = checkoutHeaderDto.PickupDateTime
        };
        foreach (var detailList in checkoutHeaderDto.CartDetails)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = detailList.ProductId.Value,
                ProductName = detailList.Product.Name,
                Price = detailList.Product.Price,
                Count = detailList.Count
            };
            orderHeader.OrderTotalItems += detailList.Count;
            orderHeader.OrderDetails.Add(orderDetail);
        }

        await _orderRepository.AddOrder(orderHeader);

        PaymentRequestMessage paymentRequestMessage = new()
        {
            Name = $"{orderHeader.FirstName} {orderHeader.LastName}",
            CardNumber = orderHeader.CardNumber,
            CVV = orderHeader.CVV,
            ExpiryMonthYear = orderHeader.ExpiryMonthYear,
            OrderId = orderHeader.Id,
            OrderTotal = orderHeader.OrderTotal
        };

        try
        {
            await _messageBus.PublishMessage(paymentRequestMessage, _orderPaymentProcessTopic);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}