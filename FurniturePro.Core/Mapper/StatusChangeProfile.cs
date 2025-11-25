using AutoMapper;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using FurniturePro.Core.Models.DTO.StatusChanges;

namespace FurniturePro.Core.Mapper;

public class StatusChangeProfile : Profile
{
    public StatusChangeProfile()
    {
        CreateMap<StatusChange, StatusChangeDTO>().ForMember(dto => dto.IdOrder,
            opt => opt.MapFrom(ent => ent.Entity1Id))
            .ForMember(dto => dto.IdStatus,
            opt => opt.MapFrom(ent => ent.Entity2Id));
        CreateMap<StatusChangeDTO, StatusChange>().ForMember(ent => ent.Entity1Id,
            opt => opt.MapFrom(dto => dto.IdOrder))
            .ForMember(ent => ent.Entity2Id,
            opt => opt.MapFrom(dto => dto.IdStatus));
    }
}
