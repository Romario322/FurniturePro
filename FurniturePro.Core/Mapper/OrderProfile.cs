using AutoMapper;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Models.DTO.Orders;

namespace FurniturePro.Core.Mapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDTO>();
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<UpdateOrderDTO, Order>();
    }
}
