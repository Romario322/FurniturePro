using AutoMapper;
using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Core.Interfaces.Repositories.Catalog;
using FurniturePro.Core.Interfaces.Services.Catalog;
using FurniturePro.Core.Interfaces.Services.Parts;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Catalog;

public class FurnitureCategoryService : BaseService<FurnitureCategory, int, FurnitureCategoryDto, CreateFurnitureCategoryDto, UpdateFurnitureCategoryDto>, IFurnitureCategoryService
{
    public FurnitureCategoryService(IFurnitureCategoryRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
