using FluentValidation;
using FluentValidation.Results;
using FurniturePro.Errors;

namespace FurniturePro.Models.ExtendedProblemDetails;

public class ValidationExceptionProblemDetails : ExtendedExceptionProblemDetails
{
    public ValidationExceptionProblemDetails(ValidationException ex, int statusCode = StatusCodes.Status400BadRequest) : base(ex, statusCode)
    {
        var errors = ex.Errors.Select(e =>
        {
            object? name = null;

            e.FormattedMessagePlaceholderValues?.TryGetValue(nameof(ValidationFailure.PropertyName), out name);
            var propertyName = name != null ? name.ToString() : e.PropertyName;

            return new
            {
                PropertyName = propertyName!,
                e.ErrorMessage
            };
        }).ToList();

        Extensions.Add("validationErrors",
            errors.Select(e => e.PropertyName).Distinct().Select(pn =>
            {
                var errorMessages = errors.Where(er => er.PropertyName == pn)
                    .Select(er => er.ErrorMessage);

                return new ValidationError(pn, errorMessages);
            }));
    }
}
