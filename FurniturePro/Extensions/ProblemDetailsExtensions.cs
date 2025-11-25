using FluentValidation;
using FurniturePro.Models.ExtendedProblemDetails;
using Microsoft.EntityFrameworkCore;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace FurniturePro.Extensions;

public static class ProblemDetailsExtensions
{
    public static void ConfigureProblemDetails(ProblemDetailsOptions options)
    {
        options.ShouldLogUnhandledException = (_, _, _) => false;
        options.OnBeforeWriteDetails = (ctx, details) =>
        {
            var logger = ctx.RequestServices.GetRequiredService<ILogger<Startup>>();

            logger.LogExceptionProblemDetails(ctx, details);

            if (details is ExtendedExceptionProblemDetails)
                return;

            var statuses = new List<int> { StatusCodes.Status401Unauthorized, StatusCodes.Status403Forbidden };
            if (statuses.Contains(details.Status ?? 0))
                details.Detail = details.Title;
        };

        options.Map<ValidationException>(ex => new ValidationExceptionProblemDetails(ex));
        options.Map<DbUpdateConcurrencyException>(ex => new ExtendedExceptionProblemDetails(ex, StatusCodes.Status409Conflict));
        options.Map<Exception>(ex => new ExtendedExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
    }
}
