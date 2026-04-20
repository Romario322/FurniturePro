using FurniturePro.Core.Entities;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class SnapshotRepository(AppDbContext context) : BaseRepository<Snapshot, int, AppDbContext>(context), ISnapshotRepository
{

}
