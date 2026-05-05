using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Furniture;
using FurniturePro.Core.Entities.Parts;

namespace FurniturePro.Core.Repositories;

public interface IFurnitureCompositionRepository : IBaseConnectionRepository<FurnitureComposition, int, Furniture, Part>
{

}
