using Mango.Web.Models;
using Mango.Web.Services.Contracts;
using static Mango.Web.StaticDetails;

namespace Mango.Web.Services;

public class ProductService: BaseService, IProductService
{
    public ProductService(IHttpClientFactory httpClient) : base(httpClient)
    {
    }

    public async Task<T> GetAllProductsAsync<T>()
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = ApiType.GET,
            Url = ProductApiBase + "/api/products",
            AccessToken = ""
        });
    }

    public async Task<T> GetProductByIdAsync<T>(int id)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = ApiType.GET,
            Url = ProductApiBase + "/api/products/" + id,
            AccessToken = ""
        });
    }

    public async Task<T> CreateProductAsync<T>(ProductDto productDto)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = ApiType.POST,
            Data = productDto,
            Url = ProductApiBase+"/api/products",
            AccessToken = ""
        });
    }

    public async Task<T> UpdateProductAsync<T>(ProductDto productDto)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = ApiType.PUT,
            Data = productDto,
            Url = ProductApiBase+"/api/products",
            AccessToken = ""
        });
    }

    public async Task<T> DeleteProductAsync<T>(int id)
    {
        return await SendAsync<T>(new ApiRequest
        {
            ApiType = ApiType.DELETE,
            Url = ProductApiBase + "/api/products/" + id,
            AccessToken = ""
        });
    }
}