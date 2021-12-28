using Mango.Web.Models;
using Mango.Web.Services.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Services;

public class BaseService : IBaseService
{
    public BaseService(IHttpClientFactory httpClient)
    {
        HttpClient = httpClient;
        ResponseModel = new ResponseDto();
    }

    public IHttpClientFactory HttpClient { get; set; }

    public ResponseDto ResponseModel { get; set; }

    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = HttpClient.CreateClient("MangoApi");
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();
            if (apiRequest.Data != null)
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8,
                    "application/json");

            message.Method = apiRequest.ApiType switch
            {
                StaticDetails.ApiType.GET => HttpMethod.Get,
                StaticDetails.ApiType.POST => HttpMethod.Post,
                StaticDetails.ApiType.PUT => HttpMethod.Put,
                StaticDetails.ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
            var apiResponse = await client.SendAsync(message);
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
            return apiResponseDto;
        }
        catch (Exception ex)
        {
            var dto = new ResponseDto
            {
                DisplayMessage = "Error",
                ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
            return apiResponseDto;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }
}