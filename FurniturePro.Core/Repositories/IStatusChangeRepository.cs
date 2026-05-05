using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Orders;

namespace FurniturePro.Core.Repositories;

public interface IStatusChangeRepository : IBaseConnectionRepository<StatusChange, int, Order, Status>
{
}
