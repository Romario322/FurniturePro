using AutoMapper;
using FurniturePro.Core.Models.DTO.Categories;

namespace FurniturePro.Core.Mapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<FurnitureCategory, CategoryDTO>();
        CreateMap<CreateCategoryDTO, FurnitureCategory>();
        CreateMap<UpdateCategoryDTO, FurnitureCategory>();
    }
}
