using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.Categories;
using FurniturePro.Core.Models.DTO.Colors;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class ColorService : IColorService
{
    private readonly IColorRepository _colorRepository;

    private readonly IMapper _mapper;

    public ColorService(IColorRepository colorRepository, IMapper mapper)
    {
        _colorRepository = colorRepository;
        _mapper = mapper;
    }

    public async Task<List<ColorDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _colorRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<ColorDTO>>(entities);
        return models;
    }

    public async Task<ColorDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _colorRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<ColorDTO>(entity);
        return model;
    }

    public async Task<DateTime?> GetLastUpdateDateAsync(CancellationToken ct = default) =>
        await _colorRepository.GetLastUpdateDateAsync(ct);

    public async Task<int> CreateAsync(CreateColorDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Color>(modelDto);
        var createdEntity = await _colorRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateColorDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _colorRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _colorRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _colorRepository.DeleteByIdAsync(id, ct);
}