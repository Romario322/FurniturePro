using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;

namespace FurniturePro.Core.Repositories;

public interface IOrderCompositionRepository : IBaseConnectionRepository<OrderComposition, int, Order, Furniture>
{
}
