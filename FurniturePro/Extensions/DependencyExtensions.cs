using FurniturePro.Core.Interfaces.Repositories.Catalog;
using FurniturePro.Core.Interfaces.Repositories.Constructor;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Core.Interfaces.Repositories.Parts;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.Catalog;
using FurniturePro.Core.Interfaces.Services.Constructor;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Services.Catalog;
using FurniturePro.Core.Services.Constructor;
using FurniturePro.Core.Services.Orders;
using FurniturePro.Core.Services.Parts;
using FurniturePro.Core.Services.System;
using FurniturePro.Infrastructure.Repositiories.Catalog;
using FurniturePro.Infrastructure.Repositiories.Constructor;
using FurniturePro.Infrastructure.Repositiories.Orders;
using FurniturePro.Infrastructure.Repositiories.Parts;
using FurniturePro.Infrastructure.Repositiories.System;

namespace FurniturePro.Extensions;

public static class DependencyExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFurnitureCategoryRepository, FurnitureCategoryRepository>();
        services.AddScoped<IFurnitureRepository, FurnitureRepository>();

        services.AddScoped<IFurniturePartRepository, FurniturePartRepository>();
        services.AddScoped<IPartRoleRepository, PartRoleRepository>();
        services.AddScoped<IReplacementGroupItemRepository, ReplacementGroupItemRepository>();
        services.AddScoped<IReplacementGroupRepository, ReplacementGroupRepository>();

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IOrderCompositionRepository, OrderCompositionRepository>();
        services.AddScoped<IOrderPartDetailRepository, OrderPartDetailRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IStatusChangeRepository, StatusChangeRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();

        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IPartCategoryRepository, PartCategoryRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IPartTypeRepository, PartTypeRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();

        services.AddScoped<IDeletedIdRepository, DeletedIdRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ISystemRoleRepository, SystemRoleRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IFurnitureCategoryService, FurnitureCategoryService>();
        services.AddScoped<IFurnitureService, FurnitureService>();

        services.AddScoped<IFurniturePartService, FurniturePartService>();
        services.AddScoped<IPartRoleService, PartRoleService>();
        services.AddScoped<IReplacementGroupItemService, ReplacementGroupItemService>();
        services.AddScoped<IReplacementGroupService, ReplacementGroupService>();

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IOrderCompositionService, OrderCompositionService>();
        services.AddScoped<IOrderPartDetailService, OrderPartDetailService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IStatusChangeService, StatusChangeService>();
        services.AddScoped<IStatusService, StatusService>();

        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IPartCategoryService, PartCategoryService>();
        services.AddScoped<IPartService, PartService>();
        services.AddScoped<IPartTypeService, PartTypeService>();
        services.AddScoped<IPriceService, PriceService>();

        services.AddScoped<IDeletedIdService, DeletedIdService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ISystemRoleService, SystemRoleService>();

        services.AddScoped<IAuthService, AuthService>();
    }
}
