using AutoMapper;
using FurniturePro.Core.Entities.Connections;
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

    public async Task<List<OrderCompositionDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _orderCompositionRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<OrderCompositionDTO>>(entities);
        return models;
    }

    public async Task<List<int>> CreateAsync(OrderCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<OrderComposition>(modelDto);
        var createdEntity = await _orderCompositionRepository.CreateAsync(entityToCreate, ct);
        return new List<int> { createdEntity.Entity1Id, createdEntity.Entity2Id };
    }

    public async Task<List<List<int>>> CreateRangeAsync(List<OrderCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        var entitiesToCreate = _mapper.Map<List<OrderComposition>>(modelsDto);
        var createdEntities = await _orderCompositionRepository.CreateRangeAsync(entitiesToCreate, ct);

        return createdEntities.Select(e => new List<int> { e.Entity1Id, e.Entity2Id }).ToList();
    }

    public async Task UpdateAsync(int id1, int id2, OrderCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _orderCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Мебель с id {id2} не найдена.");
        entity = _mapper.Map(modelDto, entity);
        await _orderCompositionRepository.UpdateAsync(entity, ct);
    }

    public async Task UpdateRangeAsync(List<OrderCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        // Получаем все сущности по их парам ID
        var entitiesToUpdate = new List<OrderComposition>();
        var missingEntities = new List<string>();

        foreach (var modelDto in modelsDto)
        {
            var entity = await _orderCompositionRepository.GetByIdsAsync(
                modelDto.IdOrder, modelDto.IdFurniture, ct);

            if (entity == null)
            {
                missingEntities.Add($"Мебель с id {modelDto.IdOrder} и Деталь с id {modelDto.IdFurniture}");
                continue;
            }

            _mapper.Map(modelDto, entity);
            entitiesToUpdate.Add(entity);
        }

        if (missingEntities.Any())
        {
            throw new Exception($"Следующие сущности не найдены:\n" +
                              string.Join("\n", missingEntities));
        }

        await _orderCompositionRepository.UpdateRangeAsync(entitiesToUpdate, ct);
    }

    public async Task DeleteAsync(int id1, int id2, CancellationToken ct = default) =>
        await _orderCompositionRepository.DeleteByIdsAsync(id1, id2, ct);

    public async Task DeleteRangeAsync(List<(int Id1, int Id2)> ids, CancellationToken ct = default) =>
        await _orderCompositionRepository.DeleteRangeByIdsAsync(ids, ct);
}