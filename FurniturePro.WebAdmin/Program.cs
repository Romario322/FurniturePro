using FurniturePro.Core;
using FurniturePro.Extensions;
using FurniturePro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. ÈÇÎËÈĞÎÂÀÍÍÎÅ ×ÒÅÍÈÅ ÔÀÉËÀ ÍÀÑÒĞÎÅÊ
var sharedConfig = new ConfigurationBuilder()
    .AddJsonFile("/usr/share/FurniturePro/appsettings.json", optional: true)
    .AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", "FurniturePro", "appsettings.json"), optional: true)
    .Build();

var appSettings = sharedConfig.GetSection(AppSettings.AppSettingsName).Get<AppSettings>();
var connectionString = appSettings?.DbOptions?.ConnectionString;

// 2. ĞÅÃÈÑÒĞÀÖÈß ÑÅĞÂÈÑÎÂ
builder.Services.AddRazorPages();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Ğåãèñòğèğóåì ÁÄ íàïğÿìóş
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}

var app = builder.Build();

// 3. ÍÀÑÒĞÎÉÊÀ PIPELINE
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