using AutoMapper;
using Mango.Services.CouponApi.DbContexts;
using Mango.Services.CouponApi.Models;
using Mango.Services.CouponApi.Models.Dto;
using Mango.Services.CouponApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponApi.Repositories;

public class CouponRepository:ICouponRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CouponRepository(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CouponDto> GetCouponByCode(string couponCode)
    {
        var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(coupon => coupon.Code==couponCode);
        return _mapper.Map<Coupon, CouponDto>(couponFromDb);
    }
}