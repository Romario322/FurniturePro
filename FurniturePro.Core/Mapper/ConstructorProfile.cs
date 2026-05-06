using AutoMapper;
using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Models.Dto.Constructor.Create;
using FurniturePro.Core.Models.Dto.Constructor.Read;
using FurniturePro.Core.Models.Dto.Constructor.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniturePro.Core.Mapper;

public class ConstructorProfile : Profile
{
    public ConstructorProfile()
    {
        CreateMap<PartRole, PartRoleDto>();
        CreateMap<CreatePartRoleDto, PartRole>();
        CreateMap<UpdatePartRoleDto, PartRole>();

        CreateMap<ReplacementGroup, ReplacementGroupDto>();
        CreateMap<CreateReplacementGroupDto, ReplacementGroup>();
        CreateMap<UpdateReplacementGroupDto, ReplacementGroup>();

        CreateMap<ReplacementGroupItem, ReplacementGroupItemDto>();
        CreateMap<CreateReplacementGroupItemDto, ReplacementGroupItem>();
        CreateMap<UpdateReplacementGroupItemDto, ReplacementGroupItem>();

        CreateMap<FurniturePart, FurniturePartDto>();
        CreateMap<CreateFurniturePartDto, FurniturePart>();
        CreateMap<UpdateFurniturePartDto, FurniturePart>();
    }
}
