using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class StatusChangeRepository(AppDbContext context) : BaseConnectionRepository<StatusChange, int, Order, Status, AppDbContext>(context), IStatusChangeRepository
{

}
