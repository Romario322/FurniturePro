using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Statuses;

namespace FurniturePro.Core.Mapper;

public class StatusProfile : Profile
{
    public StatusProfile()
    {
        CreateMap<Status, StatusDTO>();
        CreateMap<CreateStatusDTO, Status>();
        CreateMap<UpdateStatusDTO, Status>();
    }
}
