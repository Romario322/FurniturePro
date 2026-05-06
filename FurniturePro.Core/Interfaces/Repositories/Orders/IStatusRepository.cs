using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.Abstractions;

namespace FurniturePro.Core.Interfaces.Repositories.Orders;

public interface IStatusRepository : IBaseRepository<Status, StatusEnum> { }
