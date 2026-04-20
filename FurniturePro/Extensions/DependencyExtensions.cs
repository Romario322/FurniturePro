

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
        services.AddTransient<IOperationTypeRepository, OperationTypeRepository>();

        services.AddTransient<IFurnitureRepository, FurnitureRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        services.AddTransient<IPartRepository, PartRepository>();
        services.AddTransient<IDeletedIdRepository, DeletedIdRepository>();

        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOperationRepository, OperationRepository>();
        services.AddTransient<IPriceRepository, PriceRepository>();
        services.AddTransient<ISnapshotRepository, SnapshotRepository>();

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
        services.AddTransient<IOperationTypeService, OperationTypeService>();

        services.AddTransient<IFurnitureService, FurnitureService>();
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<IPartService, PartService>();
        services.AddTransient<IDeletedIdService, DeletedIdService>();

        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IOperationService, OperationService>();
        services.AddTransient<IPriceService, PriceService>();
        services.AddTransient<ISnapshotService, SnapshotService>();

        services.AddTransient<IFurnitureCompositionService, FurnitureCompositionService>();
        services.AddTransient<IOrderCompositionService, OrderCompositionService>();
        services.AddTransient<IStatusChangeService, StatusChangeService>();
    }
}
