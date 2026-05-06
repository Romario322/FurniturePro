using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Core.Interfaces.Repositories.Catalog;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Catalog;

public class FurnitureCategoryRepository(AppDbContext context)
    : BaseRepository<FurnitureCategory, int, AppDbContext>(context), IFurnitureCategoryRepository
{ }
