using Mango.Web.Models;

namespace Mango.Web.Services.Contracts;

public interface IProductService : IBaseService
{
    Task<T> GetAllProductsAsync<T>(string? token = null);
    Task<T> GetProductByIdAsync<T>(int id, string? token = null);
    Task<T> CreateProductAsync<T>(ProductDto productDto, string? token = null);
    Task<T> UpdateProductAsync<T>(ProductDto productDto, string? token = null);
    Task<T> DeleteProductAsync<T>(int id, string? token = null);
}