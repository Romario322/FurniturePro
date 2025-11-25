using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;

namespace FurniturePro.Core.Repositories;

public interface IStatusChangeRepository : IBaseConnectionRepository<StatusChange, int, Order, Status>
{
}
