using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Prices;

namespace FurniturePro.Core.Mapper;

public class PriceProfile : Profile
{
    public PriceProfile()
    {
        CreateMap<Price, PriceDTO>().ForMember(dto => dto.PartName, opt => opt.MapFrom(ent => ent.Part!.Name));
        CreateMap<CreatePriceDTO, Price>();
        CreateMap<UpdatePriceDTO, Price>();
    }
}
