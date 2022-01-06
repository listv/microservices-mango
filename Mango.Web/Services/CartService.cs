using Mango.Web.Models;
using Mango.Web.Services.Contracts;
using static Mango.Web.StaticDetails;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {

        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
        }

        public async Task<T> AddToCartAsync<T>(CartDto cart, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = $"{ShoppingCartApiBase}/api/carts/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                AccessToken = token,
                ApiType = ApiType.GET,
                Url = $"{ShoppingCartApiBase}/api/carts/GetCart/{userId}"
            });

        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.DELETE,
                Url = $"{ShoppingCartApiBase}/api/carts/RemoveCart/{cartId}",
                AccessToken = token
            });
        }

        public async Task<T> ApplyCouponAsync<T>(CartDto cart, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = $"{ShoppingCartApiBase}/api/carts/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> RemoveCouponAsync<T>(string userId, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = userId,
                Url = $"{ShoppingCartApiBase}/api/carts/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.POST,
                Data = cartHeader,
                Url = $"{ShoppingCartApiBase}/api/carts/Checkout",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cart, string? token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.PUT,
                Data = cart,
                Url = $"{ShoppingCartApiBase}/api/carts/UpdateCart",
                AccessToken = token
            });
        }
    }
}
