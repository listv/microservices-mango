namespace Mango.Services.ProductApi.Models.Dtos;

public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public object Result { get; set; } = new();
    public string DisplayMessage { get; set; } = "";
    public List<string> ErrorMessages { get; set; } = new();
}