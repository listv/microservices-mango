namespace Mango.Web;

public static class StaticDetails
{
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public static string ProductApiBase { get; set; }
}