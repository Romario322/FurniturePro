using AutoMapper;
using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Connections;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.FurnitureCompositions;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

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

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _furnitureCompositionRepository.GetLastUpdateDateAsync(ct);

    public async Task<List<int>> CreateAsync(FurnitureCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<FurnitureComposition>(modelDto);
        var createdEntity = await _furnitureCompositionRepository.CreateAsync(entityToCreate, ct);
        return new List<int> { createdEntity.Entity1Id, createdEntity.Entity2Id };
    }

    public async Task UpdateAsync(int id1, int id2, FurnitureCompositionDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _furnitureCompositionRepository.GetByIdsAsync(id1, id2, ct)
            ?? throw new Exception($"Сущность связывающая Мебель с id {id1} и Деталь с id {id2} не найдена.");
        entity = _mapper.Map(modelDto, entity);
        await _furnitureCompositionRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id1, int id2, CancellationToken ct = default) => 
        await _furnitureCompositionRepository.DeleteByIdsAsync(id1, id2, ct);
}