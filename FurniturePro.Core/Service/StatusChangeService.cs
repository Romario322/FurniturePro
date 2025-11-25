using AutoMapper;
using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Models.DTO.OrderCompositions;
using FurniturePro.Core.Models.DTO.StatusChanges;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class StatusChangeService : IStatusChangeService
{
    private readonly IStatusChangeRepository _orderCompositionRepository;

    private readonly IMapper _mapper;

    public StatusChangeService(IStatusChangeRepository orderCompositionRepository, IMapper mapper)
    {
        _orderCompositionRepository = orderCompositionRepository;
        _mapper = mapper;
    }

    public async Task<List<StatusChangeDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _orderCompositionRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<StatusChangeDTO>>(entities);
        return models;
    }

    public async Task<StatusChangeDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default)
    {
        var entity = await _orderCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Статус с id {id2} не найдена.");
        var model = _mapper.Map<StatusChangeDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _orderCompositionRepository.GetLastUpdateDateAsync(ct);

    public async Task<List<int>> CreateAsync(StatusChangeDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<StatusChange>(modelDto);
        var createdEntity = await _orderCompositionRepository.CreateAsync(entityToCreate, ct);
        return new List<int> { createdEntity.Entity1Id, createdEntity.Entity2Id };
    }

    public async Task UpdateAsync(int id1, int id2, StatusChangeDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _orderCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Статус с id {id2} не найдена.");
        entity = _mapper.Map(modelDto, entity);
        await _orderCompositionRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id1, int id2, CancellationToken ct = default) => 
        await _orderCompositionRepository.DeleteByIdsAsync(id1, id2, ct);
}