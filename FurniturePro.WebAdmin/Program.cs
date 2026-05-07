using FurniturePro.Core;
using FurniturePro.Extensions;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var sharedConfig = new ConfigurationBuilder()
    .AddJsonFile("/usr/share/FurniturePro/appsettings.json", optional: true)
    .AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", "FurniturePro", "appsettings.json"), optional: true)
    .Build();

var appSettings = sharedConfig.GetSection(AppSettings.AppSettingsName).Get<AppSettings>();
var connectionString = appSettings?.DbOptions?.ConnectionString;

builder.Services.AddRazorPages();

builder.Services.AddHttpClient("FurnitureApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5283/");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Путь к странице входа
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Путь при нехватке прав (роли)
        options.Cookie.Name = "FurniturePro.Auth";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();