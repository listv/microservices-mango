using AutoMapper;
using Mango.Services.CouponApi.Models;
using Mango.Services.CouponApi.Models.Dto;

namespace Mango.Services.CouponApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Coupon, CouponDto>().ReverseMap();
    }
}