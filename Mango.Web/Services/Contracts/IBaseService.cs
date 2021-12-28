using Mango.Web.Models;

namespace Mango.Web.Services.Contracts;

public interface IBaseService : IDisposable
{
    ResponseDto ResponseModel { get; set; }
    Task<T> SendAsync<T>(ApiRequest apiRequest);
}