using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Interfaces.Repositories.Parts;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Parts;

public class PartCategoryRepository(AppDbContext context)
    : BaseRepository<PartCategory, int, AppDbContext>(context), IPartCategoryRepository
{ }
