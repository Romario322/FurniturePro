using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;

namespace FurniturePro.Core.Interfaces.Services.Orders;

public interface IOrderCompositionService : IBaseService<int, OrderCompositionDto, CreateOrderCompositionDto, UpdateOrderCompositionDto>
{
}