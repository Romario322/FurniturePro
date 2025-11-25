using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Colors;

namespace FurniturePro.Core.Mapper;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<Color, ColorDTO>();
        CreateMap<CreateColorDTO, Color>();
        CreateMap<UpdateColorDTO, Color>();
    }
}
