using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class PartRepository(AppDbContext context) : BaseRepository<Part, int, AppDbContext>(context), IPartRepository
{

}
