using Asp.Versioning.ApiExplorer;
using FurniturePro.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FurniturePro.Models.Options;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    private readonly SwaggerConfiguration _configuration;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IServiceProvider serviceProvider)
    {
        _provider = provider;

        using var scope = serviceProvider.CreateScope();
        _configuration = scope.ServiceProvider.GetRequiredService<SwaggerConfiguration>();
    }

    public void Configure(SwaggerGenOptions options) =>
        Configure(options, _configuration);

    public void Configure(SwaggerGenOptions options, SwaggerConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, configuration));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, SwaggerConfiguration configuration)
    {
        var info = new OpenApiInfo
        {
            Title = $"{configuration.Title} {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = configuration.Description
        };

        if (description.IsDeprecated)
            info.Description += "Эта версия API устарела.";

        return info;
    }
}
