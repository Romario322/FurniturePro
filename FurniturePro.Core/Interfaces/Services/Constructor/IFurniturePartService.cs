using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.Constructor.Create;
using FurniturePro.Core.Models.Dto.Constructor.Read;
using FurniturePro.Core.Models.Dto.Constructor.Update;

namespace FurniturePro.Core.Interfaces.Services.Constructor;

public interface IFurniturePartService : IBaseService<int, FurniturePartDto, CreateFurniturePartDto, UpdateFurniturePartDto>
{
}