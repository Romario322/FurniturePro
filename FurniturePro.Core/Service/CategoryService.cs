using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _categoryRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<CategoryDTO>>(entities);
        return models;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) => 
        await _categoryRepository.GetLastUpdateDateAsync(ct);

    public async Task<CategoryDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _categoryRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<CategoryDTO>(entity);
        return model;
    }

    public async Task<int> CreateAsync(CreateCategoryDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Category>(modelDto);
        var createdEntity = await _categoryRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateCategoryDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _categoryRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _categoryRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _categoryRepository.DeleteByIdAsync(id, ct);
}