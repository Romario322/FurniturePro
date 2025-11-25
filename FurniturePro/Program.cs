using FurniturePro;
using Serilog;

var pathToContentRoot = Directory.GetCurrentDirectory();

var configuration = new ConfigurationBuilder()
    .SetBasePath(pathToContentRoot)
    .AddJsonFile("/usr/share/FurniturePro/appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseContentRoot(pathToContentRoot);
        webBuilder.UseConfiguration(configuration);
        webBuilder.UseStartup<Startup>();
    })
    .UseSerilog(logger);

host.Build().Run();
