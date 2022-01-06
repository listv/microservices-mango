namespace Mango.Services.CouponApi.Models.Dto;

public class ResponseDto
{
    public bool IsSuccess { get; set; } = true;
    public object Result { get; set; } = new();
    public string DisplayMessage { get; set; } = "";
    public List<string> ErrorMessages { get; set; } = new();
}