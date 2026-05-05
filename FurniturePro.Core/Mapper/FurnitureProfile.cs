using AutoMapper;
using FurniturePro.Core.Entities.Furniture;
using FurniturePro.Core.Models.DTO.Furnitures;

namespace FurniturePro.Core.Mapper;

public class FurnitureProfile : Profile
{
    public FurnitureProfile()
    {
        CreateMap<Furniture, FurnitureDTO>();
        CreateMap<CreateFurnitureDTO, Furniture>();
        CreateMap<UpdateFurnitureDTO, Furniture>();
    }
}
