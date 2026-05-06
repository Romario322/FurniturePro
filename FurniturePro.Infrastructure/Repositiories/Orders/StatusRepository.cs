using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Orders;

public class StatusRepository(AppDbContext context)
    : BaseRepository<Status, StatusEnum, AppDbContext>(context), IStatusRepository
{ }
