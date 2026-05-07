using AutoMapper;
using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Core.Interfaces.Repositories.Catalog;
using FurniturePro.Core.Interfaces.Services.Catalog;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Catalog;

public class FurnitureService : BaseService<Furniture, int, FurnitureDto, CreateFurnitureDto, UpdateFurnitureDto>, IFurnitureService
{
    public FurnitureService(IFurnitureRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
