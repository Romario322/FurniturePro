using AutoMapper;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class FurnitureCompositionService : IFurnitureCompositionService
{
    private readonly IFurnitureCompositionRepository _furnitureCompositionRepository;

    private readonly IMapper _mapper;

    public FurnitureCompositionService(IFurnitureCompositionRepository furnitureCompositionRepository, IMapper mapper)
    {
        _furnitureCompositionRepository = furnitureCompositionRepository;
        _mapper = mapper;
    }

    public async Task<List<FurnitureCompositionDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _furnitureCompositionRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<FurnitureCompositionDTO>>(entities);
        return models;
    }

    public async Task<FurnitureCompositionDTO?> GetByIdsAsync(int id1, int id2, CancellationToken ct = default)
    {
        var entity = await _furnitureCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Мебель с id {id1} и Деталь с id {id2} не найдена.");
        var model = _mapper.Map<FurnitureCompositionDTO>(entity);
        return model;
    }

    public async Task<List<FurnitureCompositionDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _furnitureCompositionRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<FurnitureCompositionDTO>>(entities);
        return models;
    }

    public async Task<List<int>> CreateAsync(FurnitureCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<FurnitureComposition>(modelDto);
        var createdEntity = await _furnitureCompositionRepository.CreateAsync(entityToCreate, ct);
        return new List<int> { createdEntity.Entity1Id, createdEntity.Entity2Id };
    }

    public async Task<List<List<int>>> CreateRangeAsync(List<FurnitureCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        var entitiesToCreate = _mapper.Map<List<FurnitureComposition>>(modelsDto);
        var createdEntities = await _furnitureCompositionRepository.CreateRangeAsync(entitiesToCreate, ct);

        return createdEntities.Select(e => new List<int> { e.Entity1Id, e.Entity2Id }).ToList();
    }

    public async Task UpdateAsync(int id1, int id2, FurnitureCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _furnitureCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Мебель с id {id1} и Деталь с id {id2} не найдена.");
        entity = _mapper.Map(modelDto, entity);
        await _furnitureCompositionRepository.UpdateAsync(entity, ct);
    }

    public async Task UpdateRangeAsync(List<FurnitureCompositionDTO> modelsDto, CancellationToken ct = default)
    {
        // Получаем все сущности по их парам ID
        var entitiesToUpdate = new List<FurnitureComposition>();
        var missingEntities = new List<string>();

        foreach (var modelDto in modelsDto)
        {
            var entity = await _furnitureCompositionRepository.GetByIdsAsync(
                modelDto.IdFurniture, modelDto.IdPart, ct);

            if (entity == null)
            {
                missingEntities.Add($"Мебель с id {modelDto.IdFurniture} и Деталь с id {modelDto.IdPart}");
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

        await _furnitureCompositionRepository.UpdateRangeAsync(entitiesToUpdate, ct);
    }

    public async Task DeleteAsync(int id1, int id2, CancellationToken ct = default) =>
        await _furnitureCompositionRepository.DeleteByIdsAsync(id1, id2, ct);

    public async Task DeleteRangeAsync(List<(int Id1, int Id2)> ids, CancellationToken ct = default) =>
        await _furnitureCompositionRepository.DeleteRangeByIdsAsync(ids, ct);
}