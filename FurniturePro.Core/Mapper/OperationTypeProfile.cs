using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.OperationTypes;

namespace FurniturePro.Core.Mapper;

public class OperationTypeProfile : Profile
{
    public OperationTypeProfile()
    {
        CreateMap<OperationType, OperationTypeDTO>();
        CreateMap<CreateOperationTypeDTO, OperationType>();
        CreateMap<UpdateOperationTypeDTO, OperationType>();
    }
}
