using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Prices;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class PriceService : IPriceService
{
    private readonly IPriceRepository _priceRepository;

    private readonly IMapper _mapper;

    public PriceService(IPriceRepository priceRepository, IMapper mapper)
    {
        _priceRepository = priceRepository;
        _mapper = mapper;
    }

    public async Task<List<PriceDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _priceRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<PriceDTO>>(entities);
        return models;
    }

    public async Task<PriceDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _priceRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<PriceDTO>(entity);
        return model;
    }

    public async Task<List<PriceDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _priceRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<PriceDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreatePriceDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Price>(modelDto);
        var createdEntity = await _priceRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdatePriceDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _priceRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _priceRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _priceRepository.DeleteByIdAsync(id, ct);
}