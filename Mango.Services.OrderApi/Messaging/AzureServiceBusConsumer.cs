using Azure.Messaging.ServiceBus;
using Mango.Services.OrderApi.Messages;
using Mango.Services.OrderApi.Messaging.Contracts;
using Mango.Services.OrderApi.Models;
using Mango.Services.OrderApi.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderApi.Messaging;

public class AzureServiceBusConsumer:IAzureServiceBusConsumer
{
    // private readonly IMapper _mapper;
    private readonly OrderRepository _orderRepository;

    private readonly ServiceBusProcessor _checkoutProcessor;

    public AzureServiceBusConsumer(OrderRepository orderRepository,
        //IMapper mapper,
        IConfiguration configuration)
    {
        _orderRepository = orderRepository;
        //_mapper = mapper;

        var serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
        var checkoutMessageTopic = configuration.GetValue<string>("CheckoutMessageTopic");
        var checkoutMessageSubscription = configuration.GetValue<string>("CheckoutMessageSubscription");

        var client = new ServiceBusClient(serviceBusConnectionString);
        _checkoutProcessor = client.CreateProcessor(checkoutMessageTopic, checkoutMessageSubscription);
    }

    public async Task Start()
    {
        _checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
        _checkoutProcessor.ProcessErrorAsync += ErrorHandler;
        await _checkoutProcessor.StartProcessingAsync();
    }
    
    public async Task Stop()
    {
        await _checkoutProcessor.StopProcessingAsync();
        await _checkoutProcessor.DisposeAsync();
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
        foreach(var detailList in checkoutHeaderDto.CartDetails)
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
    }
}