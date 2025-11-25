

using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Repositiories;
using UchetCartridge.Core.Services;
using UchetCartridge.Core.Services.Interfaces;

namespace FurniturePro.Extensions;

public static class DependencyExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IColorRepository, ColorRepository>();
        services.AddTransient<IMaterialRepository, MaterialRepository>();
        services.AddTransient<IStatusRepository, StatusRepository>();

        services.AddTransient<IFurnitureRepository, FurnitureRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IPartRepository, PartRepository>();

        services.AddTransient<ICountRepository, CountRepository>();
        services.AddTransient<IPriceRepository, PriceRepository>();

        services.AddTransient<IFurnitureCompositionRepository, FurnitureCompositionRepository>();
        services.AddTransient<IOrderCompositionRepository, OrderCompositionRepository>();
        services.AddTransient<IStatusChangeRepository, StatusChangeRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IColorService, ColorService>();
        services.AddTransient<IMaterialService, MaterialService>();
        services.AddTransient<IStatusService, StatusService>();

        services.AddTransient<IFurnitureService, FurnitureService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IPartService, PartService>();

        services.AddTransient<ICountService, CountService>();
        services.AddTransient<IPriceService, PriceService>();

        services.AddTransient<IFurnitureCompositionService, FurnitureCompositionService>();
        services.AddTransient<IOrderCompositionService, OrderCompositionService>();
        services.AddTransient<IStatusChangeService, StatusChangeService>();
    }
}
