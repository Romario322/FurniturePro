namespace FurniturePro.Models;

public class RequestDetails
{
    public RequestDetails(string host, bool isHttps, string method, string path, string protocol, string? contentType = null, string? queryString = null)
    {
        ContentType = contentType;
        Host = host;
        IsHttps = isHttps;
        Method = method;
        Path = path;
        Protocol = protocol;
        QueryString = queryString;
    }

    public string? ContentType { get; set; }

    public string Host { get; set; }

    public bool IsHttps { get; set; }

    public string Method { get; set; }

    public string Path { get; set; }

    public string Protocol { get; set; }

    public string? QueryString { get; set; }
}
