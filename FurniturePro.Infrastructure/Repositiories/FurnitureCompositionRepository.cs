using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Furniture;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Repositories;
using FurniturePro.Infrastructure.Data;

namespace FurniturePro.Infrastructure.Repositiories;

public class FurnitureCompositionRepository(AppDbContext context) : BaseConnectionRepository<FurnitureComposition, int, Furniture, Part, AppDbContext>(context), IFurnitureCompositionRepository
{
}
