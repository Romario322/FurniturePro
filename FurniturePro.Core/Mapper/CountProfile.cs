using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Counts;
using FurniturePro.Core.Models.DTO.Statuses;

namespace FurniturePro.Core.Mapper;

public class CountProfile : Profile
{
    public CountProfile()
    {
        CreateMap<Count, CountDTO>().ForMember(dto => dto.PartName, opt => opt.MapFrom(ent => ent.Part.Name));
        CreateMap<CreateCountDTO, Count>();
        CreateMap<UpdateCountDTO, Count>();
    }
}
