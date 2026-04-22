using FurniturePro.Extensions;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Models.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. ИЗОЛИРОВАННОЕ ЧТЕНИЕ ФАЙЛА НАСТРОЕК
// Мы создаем отдельный конфигуратор, чтобы "Urls" из файла API не переопределил порты WebAdmin
var sharedConfig = new ConfigurationBuilder()
    .AddJsonFile("/usr/share/FurniturePro/appsettings.json", optional: true)
    // Добавим относительный путь, чтобы всё работало даже в Visual Studio на Windows!
    .AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", "FurniturePro", "appsettings.json"), optional: true)
    .Build();

var appSettings = sharedConfig.GetSection(AppSettings.AppSettingsName).Get<AppSettings>();
var connectionString = appSettings?.DbOptions?.ConnectionString;


// 2. РЕГИСТРАЦИЯ СЕРВИСОВ
builder.Services.AddRazorPages();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Регистрируем БД, если строка подключения найдена
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5283");
});

var app = builder.Build();

// 3. НАСТРОЙКА PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();