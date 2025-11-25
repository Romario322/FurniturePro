using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Furnitures;
using FurniturePro.Core.Models.DTO.Statuses;

namespace FurniturePro.Core.Mapper;

public class FurnitureProfile : Profile
{
    public FurnitureProfile()
    {
        CreateMap<Furniture, FurnitureDTO>().ForMember(dto => dto.CategoryName, 
            opt => opt.MapFrom(ent => ent.Category != null ? ent.Category.Name : "Отсутствует"));
        CreateMap<CreateFurnitureDTO, Furniture>();
        CreateMap<UpdateFurnitureDTO, Furniture>();
    }
}
