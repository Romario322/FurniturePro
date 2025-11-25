using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;

namespace FurniturePro.Core.Repositories;

public interface IFurnitureCompositionRepository : IBaseConnectionRepository<FurnitureComposition, int, Furniture, Part>
{
}
