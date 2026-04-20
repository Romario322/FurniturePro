using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Operations;

namespace FurniturePro.Core.Mapper;

public class OperationProfile : Profile
{
    public OperationProfile()
    {
        CreateMap<Operation, OperationDTO>();
        CreateMap<CreateOperationDTO, Operation>();
        CreateMap<UpdateOperationDTO, Operation>();
    }
}
