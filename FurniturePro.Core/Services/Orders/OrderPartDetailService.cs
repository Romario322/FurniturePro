using AutoMapper;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Orders;

public class OrderPartDetailService : BaseService<OrderPartDetail, int, OrderPartDetailDto, CreateOrderPartDetailDto, UpdateOrderPartDetailDto>, IOrderPartDetailService
{
    public OrderPartDetailService(IOrderPartDetailRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
