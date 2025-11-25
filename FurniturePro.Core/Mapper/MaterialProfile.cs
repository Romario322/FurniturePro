using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Materials;

namespace FurniturePro.Core.Mapper;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<Material, MaterialDTO>();
        CreateMap<CreateMaterialDTO, Material>();
        CreateMap<UpdateMaterialDTO, Material>();
    }
}
