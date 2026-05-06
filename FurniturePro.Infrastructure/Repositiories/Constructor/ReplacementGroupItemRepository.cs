using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Interfaces.Repositories.Constructor;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Constructor;

public class ReplacementGroupItemRepository(AppDbContext context)
    : BaseRepository<ReplacementGroupItem, int, AppDbContext>(context), IReplacementGroupItemRepository
{ }
