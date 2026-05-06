using AutoMapper;
using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;

namespace FurniturePro.Core.Mapper;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<FurnitureCategory, FurnitureCategoryDto>();
        CreateMap<CreateFurnitureCategoryDto, FurnitureCategory>();
        CreateMap<UpdateFurnitureCategoryDto, FurnitureCategory>();

        CreateMap<Furniture, FurnitureDto>();
        CreateMap<CreateFurnitureDto, Furniture>();
        CreateMap<UpdateFurnitureDto, Furniture>();
    }
}