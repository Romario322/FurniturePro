using AutoMapper;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;

namespace FurniturePro.Core.Mapper;

public class FurnitureCompositionProfile : Profile
{
    public FurnitureCompositionProfile()
    {
        CreateMap<FurnitureComposition, FurnitureCompositionDTO>().ForMember(dto => dto.IdFurniture,
            opt => opt.MapFrom(ent => ent.Entity1Id))
            .ForMember(dto => dto.IdPart,
            opt => opt.MapFrom(ent => ent.Entity2Id));
        CreateMap<FurnitureCompositionDTO, FurnitureComposition>().ForMember(ent => ent.Entity1Id,
            opt => opt.MapFrom(dto => dto.IdFurniture))
            .ForMember(ent => ent.Entity2Id,
            opt => opt.MapFrom(dto => dto.IdPart));
    }
}
