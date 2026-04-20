using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Parts;

namespace FurniturePro.Core.Mapper;

public class PartProfile : Profile
{
    public PartProfile()
    {
        CreateMap<Part, PartDTO>();
        CreateMap<CreatePartDTO, Part>();
        CreateMap<UpdatePartDTO, Part>();
    }
}
