using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Statuses;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _statusRepository;

    private readonly IMapper _mapper;

    public StatusService(IStatusRepository statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<List<StatusDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _statusRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<StatusDTO>>(entities);
        return models;
    }

    public async Task<StatusDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _statusRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<StatusDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _statusRepository.GetLastUpdateDateAsync(ct);

    public async Task<int> CreateAsync(CreateStatusDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Status>(modelDto);
        var createdEntity = await _statusRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateStatusDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _statusRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _statusRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _statusRepository.DeleteByIdAsync(id, ct);
}