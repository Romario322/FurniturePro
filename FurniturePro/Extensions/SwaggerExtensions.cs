using Asp.Versioning.ApiExplorer;
using FurniturePro.Extensions;
using FurniturePro.Models.Options;
using FurniturePro.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FurniturePro.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services, SwaggerConfiguration configuration)
    {
        services.AddScoped(_ => configuration);
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(options =>
        {
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();
        });

        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static void UseSwagger(this IApplicationBuilder app,
        IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider,
        SwaggerConfiguration configuration)
    {
        app.UseSwagger(so =>
        {
            if (!env.IsDevelopment() && !string.IsNullOrEmpty(configuration.ProxyPass))
            {
                so.PreSerializeFilters.Add((swaggerDoc, _) => swaggerDoc.Servers = new List<OpenApiServer>
                {
                    new() {Url = configuration.ProxyPass}
                });
            }
        });
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
    }
}
