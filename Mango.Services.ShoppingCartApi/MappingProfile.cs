using AutoMapper;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dto;

namespace Mango.Services.ShoppingCartApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
        CreateMap<CartDetailDto, CartDetail>().ReverseMap();
        CreateMap<CartDto, Cart>().ReverseMap();
    }
}