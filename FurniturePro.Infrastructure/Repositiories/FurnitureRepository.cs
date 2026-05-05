using FurniturePro.Core.Entities.Furniture;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class FurnitureRepository(AppDbContext context) : BaseRepository<Furniture, int, AppDbContext>(context), IFurnitureRepository
{

}
