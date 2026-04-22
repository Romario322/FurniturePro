using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Orders;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _orderRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<OrderDTO>>(entities);
        return models;
    }

    public async Task<OrderDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _orderRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<OrderDTO>(entity);
        return model;
    }

    public async Task<List<OrderDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _orderRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<OrderDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreateOrderDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Order>(modelDto);
        var createdEntity = await _orderRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateOrderDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _orderRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _orderRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _orderRepository.DeleteByIdAsync(id, ct);
}