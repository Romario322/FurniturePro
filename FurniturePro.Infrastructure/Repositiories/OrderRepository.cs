using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class OrderRepository(AppDbContext context) : BaseRepository<Order, int, AppDbContext>(context), IOrderRepository
{

}
