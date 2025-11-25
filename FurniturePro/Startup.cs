using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation.AspNetCore;
using FurniturePro.Extensions;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Models.Settings;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace FurniturePro;

public class Startup
{
    private readonly AppSettings _appSettings;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        HostingEnvironment = env;

        _appSettings = Configuration.GetSection(AppSettings.AppSettingsName).Get<AppSettings>()
            ?? throw new ValidationException("Блок с настройками AppSettings не найден");

        _appSettings.Validate();
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment HostingEnvironment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddProblemDetails(Extensions.ProblemDetailsExtensions.ConfigureProblemDetails);

        services.AddCors(options => options.AddPolicy("AllowRazorPages", policy => policy.WithOrigins("http://localhost:5283")
                      .AllowAnyHeader()
                      .AllowAnyMethod()));
        services.AddRepositories();
        services.AddServices();

        services.AddControllers()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
            });

        services.AddFluentValidationAutoValidation();

        services.ConfigureValidationException();

        services.AddApiVersioning(o =>
        {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        if (_appSettings.Swagger is { UseSwagger: true })
            services.AddSwagger(_appSettings.Swagger);

        services.AddHttpContextAccessor();

        if (_appSettings.DbOptions.UseInMemory.HasValue && _appSettings.DbOptions.UseInMemory.Value)
        {
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("page_template"));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_appSettings.DbOptions.ConnectionString));
        }

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, AppDbContext dbContext)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        if (_appSettings.DbOptions.UseInMemory.HasValue && _appSettings.DbOptions.UseInMemory.Value)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        }
        else
        {
            app.UseCors();
        }

        app.UseProblemDetails();
        app.UseRequestLocalization();
        app.UseCors("AllowRazorPages");
        app.UseRouting();

        //app.UseAuthentication();
        //app.UseAuthorization();

        if (_appSettings.Swagger is { UseSwagger: true })
            app.UseSwagger(env, provider, _appSettings.Swagger);

        app.UseEndpoints(endpoints => endpoints.MapControllers().AllowAnonymous());
    }
}