using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Furnitures;
using FurniturePro.Core.Models.DTO.Statuses;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class FurnitureService : IFurnitureService
{
    private readonly IFurnitureRepository _furnitureRepository;

    private readonly IMapper _mapper;

    public FurnitureService(IFurnitureRepository furnitureRepository, IMapper mapper)
    {
        _furnitureRepository = furnitureRepository;
        _mapper = mapper;
    }

    public async Task<List<FurnitureDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _furnitureRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<FurnitureDTO>>(entities);
        return models;
    }

    public async Task<FurnitureDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _furnitureRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<FurnitureDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _furnitureRepository.GetLastUpdateDateAsync(ct);

    public async Task<int> CreateAsync(CreateFurnitureDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Furniture>(modelDto);
        var createdEntity = await _furnitureRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateFurnitureDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _furnitureRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _furnitureRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _furnitureRepository.DeleteByIdAsync(id, ct);
}