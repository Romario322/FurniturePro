using AutoMapper;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Models.Dto.Parts.Create;
using FurniturePro.Core.Models.Dto.Parts.Read;
using FurniturePro.Core.Models.Dto.Parts.Update;

namespace FurniturePro.Core.Mapper;

public class PartsProfile : Profile
{
    public PartsProfile()
    {
        CreateMap<Color, ColorDto>();
        CreateMap<CreateColorDto, Color>();
        CreateMap<UpdateColorDto, Color>();

        CreateMap<Material, MaterialDto>();
        CreateMap<CreateMaterialDto, Material>();
        CreateMap<UpdateMaterialDto, Material>();

        CreateMap<PartCategory, PartCategoryDto>();
        CreateMap<CreatePartCategoryDto, PartCategory>();
        CreateMap<UpdatePartCategoryDto, PartCategory>();

        CreateMap<PartType, PartTypeDto>();
        CreateMap<CreatePartTypeDto, PartType>();
        CreateMap<UpdatePartTypeDto, PartType>();

        CreateMap<Price, PriceDto>();
        CreateMap<CreatePriceDto, Price>();
        CreateMap<UpdatePriceDto, Price>();

        CreateMap<Part, PartDto>();
        CreateMap<CreatePartDto, Part>();
        CreateMap<UpdatePartDto, Part>();
    }
}