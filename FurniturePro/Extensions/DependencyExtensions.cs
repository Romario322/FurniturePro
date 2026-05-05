namespace FurniturePro.Extensions;

public static class DependencyExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        //services.AddTransient<ICategoryRepository, CategoryRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        //services.AddTransient<ICategoryService, CategoryService>();
    }
}
