using AutoMapper;
using Mango.Services.ShoppingCartApi.DbContexts;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dto;
using Mango.Services.ShoppingCartApi.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CartRepository(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CartDto> GetCartByUserId(string userId)
    {
        var cart = new Cart
        {
            Header = await _db.CartHeaders.FirstOrDefaultAsync(header => header.UserId == userId) ??
                     throw new KeyNotFoundException()
        };

        cart.Details = _db.CartDetails.Where(detail => detail.CartHeaderId == cart.Header.Id)
            .Include(detail => detail.Product);

        return _mapper.Map<Cart, CartDto>(cart);
    }

    public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
    {
        var cart = _mapper.Map<CartDto, Cart>(cartDto);
        var productInDb = await _db.Products.FirstOrDefaultAsync(product =>
            product.Id == cartDto.Details.FirstOrDefault()!.ProductId);
        
        if (productInDb == null)
        {
            _db.Products.Add(cart.Details.FirstOrDefault()!.Product);
            await _db.SaveChangesAsync();
        }

        var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
            .FirstOrDefaultAsync(header => header.UserId == cart.Header.UserId);

        if (cartHeaderFromDb == null)
        {
            _db.CartHeaders.Add(cart.Header);
            await _db.SaveChangesAsync();
            cart.Details.FirstOrDefault()!.CartHeaderId = cart.Header.Id;
            cart.Details.FirstOrDefault()!.Product = null!;
            _db.CartDetails.Add(cart.Details.FirstOrDefault()!);
            await _db.SaveChangesAsync();
        }
        else
        {
            var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(detail =>
                detail.ProductId == cart.Details.FirstOrDefault()!.ProductId &&
                detail.CartHeaderId == cartHeaderFromDb.Id);

            if (cartDetailsFromDb == null)
            {
                cart.Details.FirstOrDefault()!.CartHeaderId = cartHeaderFromDb.Id;
                cart.Details.FirstOrDefault()!.Product = null!;
                _db.CartDetails.Add(cart.Details.FirstOrDefault()!);
                await _db.SaveChangesAsync();
            }
            else
            {
                cart.Details.FirstOrDefault()!.Product = null!;
                cart.Details.FirstOrDefault()!.Count += cartDetailsFromDb.Count;
                cart.Details.FirstOrDefault()!.Id = cartDetailsFromDb.Id;
                cart.Details.FirstOrDefault()!.CartHeaderId = cartDetailsFromDb.CartHeaderId;
                _db.CartDetails.Update(cart.Details.FirstOrDefault()!);
                await _db.SaveChangesAsync();
            }
        }

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<bool> RemoveFromCart(int cartDetailsId)
    {
        try
        {
            CartDetail cartDetail =
                await _db.CartDetails.FirstOrDefaultAsync(detail => detail.Id == cartDetailsId) ??
                throw new KeyNotFoundException();

            _db.CartDetails.Remove(cartDetail);

            int totalCountOfCartItems =
                _db.CartDetails.Count(detail => detail.CartHeaderId == cartDetail.CartHeaderId);

            if (totalCountOfCartItems == 1)
            {
                var cartHeaderToRemove =
                    await _db.CartHeaders.FirstOrDefaultAsync(header => header.Id == cartDetail.CartHeaderId) ??
                    throw new KeyNotFoundException();
                _db.CartHeaders.Remove(cartHeaderToRemove);
            }

            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(header => header.UserId==userId);
        
        if (cartHeaderFromDb == null) return false;
        
        _db.CartDetails.RemoveRange(_db.CartDetails.Where(detail => detail.CartHeaderId == cartHeaderFromDb.Id));
        _db.CartHeaders.Remove(cartHeaderFromDb);
        await _db.SaveChangesAsync();
        
        return true;
    }
}