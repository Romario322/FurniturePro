using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class CategoryRepository(AppDbContext context) : BaseRepository<FurnitureCategory, int, AppDbContext>(context), ICategoryRepository
{

}
