using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class MaterialRepository(AppDbContext context) : BaseRepository<Material, int, AppDbContext>(context), IMaterialRepository
{

}
