using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class CategoryRepository(AppDbContext context) : BaseRepository<Category, int, AppDbContext>(context), ICategoryRepository
{

}
