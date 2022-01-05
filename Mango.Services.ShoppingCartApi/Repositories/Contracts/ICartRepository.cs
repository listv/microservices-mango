using Mango.Services.ShoppingCartApi.Models.Dto;

namespace Mango.Services.ShoppingCartApi.Repositories.Contracts;

public interface ICartRepository
{
    Task<CartDto> GetCartByUserId(string userId);
    Task<CartDto> CreateUpdateCart(CartDto cartDto);
    Task<bool> RemoveFromCart(int cartDetailsId);
    Task<bool> ClearCart(string userId);
}