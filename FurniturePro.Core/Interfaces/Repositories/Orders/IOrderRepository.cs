using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Interfaces.Repositories.Abstractions;

namespace FurniturePro.Core.Interfaces.Repositories.Orders;

public interface IOrderRepository : IBaseRepository<Order, int> { }
