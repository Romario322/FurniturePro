using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.DeletedIds;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class DeletedIdService : IDeletedIdService
{
    private readonly IDeletedIdRepository _deletedIdRepository;

    private readonly IMapper _mapper;

    public DeletedIdService(IDeletedIdRepository deletedIdRepository, IMapper mapper)
    {
        _deletedIdRepository = deletedIdRepository;
        _mapper = mapper;
    }

    public async Task<List<DeletedIdDTO>> GetAllAsync(string tableName, CancellationToken ct)
    {
        var entities = await _deletedIdRepository.GetAllAsync(tableName, ct);

        var models = _mapper.Map<List<DeletedIdDTO>>(entities);
        return models;
    }

    public async Task<List<DeletedIdDTO>> GetAfterDateAsync(string dateTime, string tableName, CancellationToken ct = default)
    {
        var entities = await _deletedIdRepository.GetAfterDateAsync(dateTime, tableName, ct);

        var models = _mapper.Map<List<DeletedIdDTO>>(entities);
        return models;
    }

    public async Task<DeletedIdDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _deletedIdRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<DeletedIdDTO>(entity);
        return model;
    }

    public async Task<int> CreateAsync(CreateDeletedIdDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<DeletedId>(modelDto);
        var createdEntity = await _deletedIdRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task<List<int>> CreateRangeAsync(List<CreateDeletedIdDTO> modelsDto, CancellationToken ct = default)
    {
        var entitiesToCreate = _mapper.Map<List<DeletedId>>(modelsDto);
        var createdEntities = await _deletedIdRepository.CreateRangeAsync(entitiesToCreate, ct);

        return createdEntities.Select(e => e.Id).ToList();
    }

    public async Task UpdateAsync(int id, UpdateDeletedIdDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _deletedIdRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _deletedIdRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _deletedIdRepository.DeleteByIdAsync(id, ct);
}