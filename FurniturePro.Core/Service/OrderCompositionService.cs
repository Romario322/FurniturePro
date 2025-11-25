using AutoMapper;
using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class OrderCompositionService : IOrderCompositionService
{
    private readonly IOrderCompositionRepository _orderCompositionRepository;

    private readonly IMapper _mapper;

    public OrderCompositionService(IOrderCompositionRepository orderCompositionRepository, IMapper mapper)
    {
        _orderCompositionRepository = orderCompositionRepository;
        _mapper = mapper;
    }

    public async Task<List<OrderCompositionDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _orderCompositionRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<OrderCompositionDTO>>(entities);
        return models;
    }

    public async Task<OrderCompositionDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default)
    {
        var entity = await _orderCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Мебель с id {id2} не найдена.");
        var model = _mapper.Map<OrderCompositionDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _orderCompositionRepository.GetLastUpdateDateAsync(ct);

    public async Task<List<int>> CreateAsync(OrderCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<OrderComposition>(modelDto);
        var createdEntity = await _orderCompositionRepository.CreateAsync(entityToCreate, ct);
        return new List<int> { createdEntity.Entity1Id, createdEntity.Entity2Id };
    }

    public async Task UpdateAsync(int id1, int id2, OrderCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _orderCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Мебель с id {id2} не найдена.");
        entity = _mapper.Map(modelDto, entity);
        await _orderCompositionRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id1, int id2, CancellationToken ct = default) => 
        await _orderCompositionRepository.DeleteByIdsAsync(id1, id2, ct);
}