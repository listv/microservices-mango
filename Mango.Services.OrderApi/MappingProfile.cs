using AutoMapper;
using Mango.Services.OrderApi.Messages;
using Mango.Services.OrderApi.Models;

namespace Mango.Services.OrderApi;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<CheckoutHeaderDto, OrderHeader>()
            .ForMember(header => header.OrderDateTime, expression => expression.MapFrom(dto => DateTime.Now))
            .ForMember(header => header.PaymentStatus, expression => expression.MapFrom(dto => false))
            .ForMember(header => header.OrderTotalItems,
                expression => expression.MapFrom(dto => dto.CartDetails.Sum(detailDto => detailDto.Count)));

        CreateMap<List<CartDetailDto>, List<OrderDetail>>();

        CreateMap<CartDetailDto, OrderDetail>()
            .ForMember(detail => detail.ProductName, expression => expression.MapFrom(dto => dto.Product.Name))
            .ForMember(detail => detail.Price, expression => expression.MapFrom(dto => dto.Product.Price));
    }
}