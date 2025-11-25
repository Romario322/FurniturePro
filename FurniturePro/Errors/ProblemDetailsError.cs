using FurniturePro.Models;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Errors;

/// <summary>
/// Объект ошибки Http-запроса для логирования.
/// </summary>
public class ProblemDetailsError
{
    /// <summary>
    /// Создает новый экземпляр класса <see cref="ProblemDetailsError"/>.
    /// </summary>
    public ProblemDetailsError(RequestDetails request, ProblemDetails error)
    {
        Request = request;
        Error = error;
    }

    /// <summary>
    /// Возвращает или задает запрос, в котором произошла ошибка.
    /// </summary>
    public RequestDetails Request { get; set; }

    /// <summary>
    /// Возвращает или задает объект ошибки.
    /// </summary>
    public ProblemDetails Error { get; set; }
}
