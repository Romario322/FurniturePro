using FurniturePro.Models.Settings;
using FurniturePro.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurniturePro.Extensions;

public static class AppSettingsExtensions
{
    public static void Validate(this AppSettings appSettings)
    {
        var validationResults = new List<ValidationResult>();
        if (!RecursiveValidator.TryValidateObjectRecursive(appSettings, validationResults))
        {
            var validationMessage = ValidationResultsToString(validationResults);
            throw new ValidationException(validationMessage);
        }
    }

    private static string ValidationResultsToString(IEnumerable<ValidationResult> validationResults)
    {
        var stringBuilder = new StringBuilder("Ошибки заполнения AppSettings: ");

        foreach (var validationResult in validationResults)
        {
            stringBuilder.Append(validationResult.ToString());
            stringBuilder.Append("; ");
        }

        return stringBuilder.ToString();
    }
}
