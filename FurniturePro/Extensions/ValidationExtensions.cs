using FluentValidation;
using FluentValidation.AspNetCore;
using FurniturePro.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePro.Extensions;

public static class ValidationExtensions
{
    public static void ConfigureValidationException(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(o => o.InvalidModelStateResponseFactory = actionContext =>
        {
            var modelState = actionContext.ModelState;
            var failures = new List<FluentValidation.Results.ValidationFailure>();
            foreach (var (key, value) in modelState)
            {
                var errors = value.Errors;
                if (errors.Count > 0)
                {
                    failures.AddRange(from error in errors
                                      select error.ErrorMessage
                        into errorMessage
                                      where !string.IsNullOrEmpty(errorMessage)
                                      select new FluentValidation.Results.ValidationFailure(key, errorMessage));
                }
            }

            throw new ValidationException("Validation failed", failures);
        });

        services.AddTransient<IValidatorInterceptor, DefaultValidatorInterceptor>();
    }
}
