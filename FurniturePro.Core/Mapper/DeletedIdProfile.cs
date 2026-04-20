using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.DeletedIds;

namespace FurniturePro.Core.Mapper;

public class DeletedIdProfile : Profile
{
    public DeletedIdProfile()
    {
        CreateMap<DeletedId, DeletedIdDTO>();
        CreateMap<CreateDeletedIdDTO, DeletedId>();
        CreateMap<UpdateDeletedIdDTO, DeletedId>();
    }
}
