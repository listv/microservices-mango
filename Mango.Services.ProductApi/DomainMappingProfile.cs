using AutoMapper;
using Mango.Services.ProductApi.Models;
using Mango.Services.ProductApi.Models.Dtos;

namespace Mango.Services.ProductApi;

public class DomainMappingProfile : Profile
{
    public DomainMappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>()
            .ForMember(product => product.CategoryId, expression => expression.Ignore());

        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>()
            .ForMember(category => category.Products, expression => expression.Ignore());
    }
}