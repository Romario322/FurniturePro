using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
namespace FurniturePro.Models.ExtendedProblemDetails;

public class ExtendedExceptionProblemDetails : ProblemDetails
{
    public ExtendedExceptionProblemDetails(Exception ex, int statusCode)
    {
        Detail = ex.Message;
        Status = statusCode;
        Title = ReasonPhrases.GetReasonPhrase(statusCode);
        Type = $"https://httpstatuses.com/{statusCode}";
        Exception = ex;
    }

    /// <summary>Возвращает исключение.</summary>
    [JsonIgnore]
    public Exception Exception { get; }
}
