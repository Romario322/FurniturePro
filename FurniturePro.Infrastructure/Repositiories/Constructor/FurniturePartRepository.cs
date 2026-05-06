using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Interfaces.Repositories.Constructor;
using FurniturePro.Infrastructure.Data;
using FurniturePro.Infrastructure.Repositiories.Abstractions;

namespace FurniturePro.Infrastructure.Repositiories.Constructor;

public class FurniturePartRepository(AppDbContext context)
    : BaseRepository<FurniturePart, int, AppDbContext>(context), IFurniturePartRepository
{ }
