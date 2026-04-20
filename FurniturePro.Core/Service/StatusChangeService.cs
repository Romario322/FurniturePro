using AutoMapper;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Models.DTO.StatusChanges;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class StatusChangeService : IStatusChangeService
{
    private readonly IStatusChangeRepository _statusChangeRepository;

    private readonly IMapper _mapper;

    public StatusChangeService(IStatusChangeRepository statusChangeRepository, IMapper mapper)
    {
        _statusChangeRepository = statusChangeRepository;
        _mapper = mapper;
    }

    public async Task<List<StatusChangeDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _statusChangeRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<StatusChangeDTO>>(entities);
        return models;
    }

    public async Task<StatusChangeDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default)
    {
        var entity = await _statusChangeRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Статус с id {id2} не найдена.");
        var model = _mapper.Map<StatusChangeDTO>(entity);
        return model;
    }

    public async Task<List<StatusChangeDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _statusChangeRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<StatusChangeDTO>>(entities);
        return models;
    }

    public async Task<List<int>> CreateAsync(StatusChangeDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<StatusChange>(modelDto);
        var createdEntity = await _statusChangeRepository.CreateAsync(entityToCreate, ct);
        return new List<int> { createdEntity.Entity1Id, createdEntity.Entity2Id };
    }

    public async Task UpdateAsync(int id1, int id2, StatusChangeDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _statusChangeRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Заказ с id {id1} и Статус с id {id2} не найдена.");
        entity = _mapper.Map(modelDto, entity);
        await _statusChangeRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id1, int id2, CancellationToken ct = default) =>
        await _statusChangeRepository.DeleteByIdsAsync(id1, id2, ct);
}