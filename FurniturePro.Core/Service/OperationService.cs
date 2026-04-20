using AutoMapper;
using FurniturePro.Core.Entities;
using FurniturePro.Core.Models.DTO.Operations;
using FurniturePro.Core.Repositories;
using UchetCartridge.Core.Services.Interfaces;

namespace UchetCartridge.Core.Services;

public class OperationService : IOperationService
{
    private readonly IOperationRepository _operationRepository;

    private readonly IMapper _mapper;

    public OperationService(IOperationRepository operationRepository, IMapper mapper)
    {
        _operationRepository = operationRepository;
        _mapper = mapper;
    }

    public async Task<List<OperationDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _operationRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<OperationDTO>>(entities);
        return models;
    }

    public async Task<OperationDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _operationRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<OperationDTO>(entity);
        return model;
    }

    public async Task<List<OperationDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _operationRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<OperationDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreateOperationDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<Operation>(modelDto);
        var createdEntity = await _operationRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateOperationDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _operationRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _operationRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _operationRepository.DeleteByIdAsync(id, ct);
}