using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Counts;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Models.DTO.Statuses;

namespace FurniturePro.Core.Mapper;

public class PartProfile : Profile
{
    public PartProfile()
    {
        CreateMap<Part, PartDTO>().ForMember(dto => dto.ColorName, 
            opt => opt.MapFrom(ent => ent.Color != null ? ent.Color.Name : "Отсутствует"))
            .ForMember(dto => dto.ColorName,
            opt => opt.MapFrom(ent => ent.Material != null ? ent.Material.Name : "Отсутствует"));
        CreateMap<CreatePartDTO, Part>();
        CreateMap<UpdatePartDTO, Part>();
    }
}
