using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Orders;

public class OrderCompositionRepository(AppDbContext context)
    : BaseRepository<OrderComposition, int, AppDbContext>(context), IOrderCompositionRepository
{ }
