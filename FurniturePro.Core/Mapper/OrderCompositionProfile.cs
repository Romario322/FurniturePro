using AutoMapper;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.OrderCompositions;

namespace FurniturePro.Core.Mapper;

public class OrderCompositionProfile : Profile
{
    public OrderCompositionProfile()
    {
        CreateMap<OrderComposition, OrderCompositionDTO>().ForMember(dto => dto.IdOrder,
            opt => opt.MapFrom(ent => ent.Entity1Id))
            .ForMember(dto => dto.IdFurniture,
            opt => opt.MapFrom(ent => ent.Entity2Id));
        CreateMap<OrderCompositionDTO, OrderComposition>().ForMember(ent => ent.Entity1Id,
            opt => opt.MapFrom(dto => dto.IdOrder))
            .ForMember(ent => ent.Entity2Id,
            opt => opt.MapFrom(dto => dto.IdFurniture));
    }
}
