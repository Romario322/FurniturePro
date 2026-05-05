using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class StatusRepository(AppDbContext context) : BaseRepository<Status, int, AppDbContext>(context), IStatusRepository
{

}
