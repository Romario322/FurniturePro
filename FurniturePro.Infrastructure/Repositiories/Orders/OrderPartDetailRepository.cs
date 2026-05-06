using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Orders;

public class OrderPartDetailRepository(AppDbContext context)
    : BaseRepository<OrderPartDetail, int, AppDbContext>(context), IOrderPartDetailRepository
{ }
