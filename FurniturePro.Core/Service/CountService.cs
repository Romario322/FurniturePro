using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Counts;
using FurniturePro.Core.Models.DTO.Statuses;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class CountService : ICountService
{
    private readonly ICountRepository _сountRepository;

    private readonly IMapper _mapper;

    public CountService(ICountRepository сountRepository, IMapper mapper)
    {
        _сountRepository = сountRepository;
        _mapper = mapper;
    }

    public async Task<List<CountDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _сountRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<CountDTO>>(entities);
        return models;
    }

    public async Task<CountDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _сountRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<CountDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _сountRepository.GetLastUpdateDateAsync(ct);

    public async Task<int> CreateAsync(CreateCountDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Count>(modelDto);
        var createdEntity = await _сountRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateCountDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _сountRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _сountRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _сountRepository.DeleteByIdAsync(id, ct);
}