using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;

namespace FurniturePro.Core.Interfaces.Services.Catalog;

public interface IFurnitureCategoryService : IBaseService<int, FurnitureCategoryDto, CreateFurnitureCategoryDto, UpdateFurnitureCategoryDto>
{
}
