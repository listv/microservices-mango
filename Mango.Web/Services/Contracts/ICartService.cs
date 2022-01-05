using Mango.Web.Models;

namespace Mango.Web.Services.Contracts;

public interface ICartService : IBaseService
{
    Task<T> GetCartByUserIdAsync<T>(string userId, string? token);
    Task<T> AddToCartAsync<T>(CartDto cart, string? token);
    Task<T> UpdateCartAsync<T>(CartDto cart, string? token);
    Task<T> RemoveFromCartAsync<T>(int cartId, string? token);
}

