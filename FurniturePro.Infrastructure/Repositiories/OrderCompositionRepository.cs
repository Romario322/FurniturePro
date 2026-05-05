using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Furniture;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class OrderCompositionRepository(AppDbContext context) : BaseConnectionRepository<OrderComposition, int, Order, Furniture, AppDbContext>(context), IOrderCompositionRepository
{

}
