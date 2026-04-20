using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;

namespace FurniturePro.Core.Mapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDTO>();
        CreateMap<CreateCategoryDTO, Category>();
        CreateMap<UpdateCategoryDTO, Category>();
    }
}
