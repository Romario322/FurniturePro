using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Materials;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class MaterialService : IMaterialService
{
    private readonly IMaterialRepository _materialRepository;

    private readonly IMapper _mapper;

    public MaterialService(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<List<MaterialDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _materialRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<MaterialDTO>>(entities);
        return models;
    }

    public async Task<MaterialDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _materialRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<MaterialDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _materialRepository.GetLastUpdateDateAsync(ct);

    public async Task<int> CreateAsync(CreateMaterialDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Material>(modelDto);
        var createdEntity = await _materialRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateMaterialDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _materialRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _materialRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _materialRepository.DeleteByIdAsync(id, ct);
}