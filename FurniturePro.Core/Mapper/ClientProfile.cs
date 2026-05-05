using AutoMapper;
using FurniturePro.Core.Entities.Users;
using FurniturePro.Core.Models.DTO.Clients;

namespace FurniturePro.Core.Mapper;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDTO>();
        CreateMap<CreateClientDTO, Client>();
        CreateMap<UpdateClientDTO, Client>();
    }
}
