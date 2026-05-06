using AutoMapper;
using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Models.Dto.Catalog.Create;
using FurniturePro.Core.Models.Dto.Catalog.Read;
using FurniturePro.Core.Models.Dto.Catalog.Update;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;

namespace FurniturePro.Core.Mapper;

public class OrdersProfile : Profile
{
    public OrdersProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();

        CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();

        CreateMap<OrderComposition, OrderCompositionDto>();
        CreateMap<CreateOrderCompositionDto, OrderComposition>();
        CreateMap<UpdateOrderCompositionDto, OrderComposition>();

        CreateMap<OrderPartDetail, OrderPartDetailDto>();
        CreateMap<CreateOrderPartDetailDto, OrderPartDetail>();
        CreateMap<UpdateOrderPartDetailDto, OrderPartDetail>();

        CreateMap<Status, StatusDto>();
        CreateMap<CreateStatusDto, Status>();
        CreateMap<UpdateStatusDto, Status>();

        CreateMap<StatusChange, StatusChangeDto>();
        CreateMap<CreateStatusChangeDto, StatusChange>();
        CreateMap<UpdateStatusChangeDto, StatusChange>();
    }
}