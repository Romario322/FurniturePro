using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Counts;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Models.DTO.Statuses;

namespace FurniturePro.Core.Mapper;

public class PriceProfile : Profile
{
    public PriceProfile()
    {
        CreateMap<Price, PriceDTO>().ForMember(dto => dto.PartName, opt => opt.MapFrom(ent => ent.Part.Name));
        CreateMap<CreatePriceDTO, Price>();
        CreateMap<UpdatePriceDTO, Price>();
    }
}
