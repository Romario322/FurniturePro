using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Entities.Parts;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Parts;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class PartService : IPartService
{
    private readonly IPartRepository _partRepository;

    private readonly IMapper _mapper;

    public PartService(IPartRepository partRepository, IMapper mapper)
    {
        _partRepository = partRepository;
        _mapper = mapper;
    }

    public async Task<List<PartDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _partRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<PartDTO>>(entities);
        return models;
    }

    public async Task<PartDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _partRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<PartDTO>(entity);
        return model;
    }

    public async Task<List<PartDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _partRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<PartDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreatePartDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Part>(modelDto);
        var createdEntity = await _partRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task<List<int>> CreateRangeAsync(List<CreatePartDTO> modelDtos, CancellationToken ct = default)
    {
        var entitiesToCreate = _mapper.Map<List<Part>>(modelDtos);
        var createdEntities = await _partRepository.CreateRangeAsync(entitiesToCreate, ct);
        return createdEntities.Select(ent => ent.Id).ToList();
    }

    public async Task UpdateAsync(int id, UpdatePartDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _partRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _partRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _partRepository.DeleteByIdAsync(id, ct);
}