using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Snapshots;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class SnapshotService : ISnapshotService
{
    private readonly ISnapshotRepository _snapshotRepository;

    private readonly IMapper _mapper;

    public SnapshotService(ISnapshotRepository snapshotRepository, IMapper mapper)
    {
        _snapshotRepository = snapshotRepository;
        _mapper = mapper;
    }

    public async Task<List<SnapshotDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _snapshotRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<SnapshotDTO>>(entities);
        return models;
    }

    public async Task<SnapshotDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _snapshotRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<SnapshotDTO>(entity);
        return model;
    }

    public async Task<List<SnapshotDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _snapshotRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<SnapshotDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreateSnapshotDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Snapshot>(modelDto);
        var createdEntity = await _snapshotRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateSnapshotDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _snapshotRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _snapshotRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _snapshotRepository.DeleteByIdAsync(id, ct);
}