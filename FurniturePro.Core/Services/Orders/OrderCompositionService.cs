using AutoMapper;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Orders;

public class OrderCompositionService : BaseService<OrderComposition, int, OrderCompositionDto, CreateOrderCompositionDto, UpdateOrderCompositionDto>, IOrderCompositionService
{
    public OrderCompositionService(IOrderCompositionRepository repository, ICurrentUserService currentUserService, IDeletedIdRepository deletedIdRepository, IMapper mapper)
        : base(repository, currentUserService, deletedIdRepository, mapper)
    {
    }
}
