using AutoMapper;
using FurniturePro.Core.Entities.Parts;
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
