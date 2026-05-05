using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Furniture;
using FurniturePro.Core.Entities.Orders;

namespace FurniturePro.Core.Repositories;

public interface IOrderCompositionRepository : IBaseConnectionRepository<OrderComposition, int, Order, Furniture>
{
}
