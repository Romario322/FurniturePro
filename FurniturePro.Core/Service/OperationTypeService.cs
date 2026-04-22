using AutoMapper;
using FurniturePro.Core.Entities.Dictionaries;
using FurniturePro.Core.Models.DTO.OperationTypes;
using FurniturePro.Core.Repositories;
using FurniturePro.Core.Services.Interfaces;

namespace FurniturePro.Core.Services;

public class OperationTypeService : IOperationTypeService
{
    private readonly IOperationTypeRepository _operationTypeRepository;

    private readonly IMapper _mapper;

    public OperationTypeService(IOperationTypeRepository operationTypeRepository, IMapper mapper)
    {
        _operationTypeRepository = operationTypeRepository;
        _mapper = mapper;
    }

    public async Task<List<OperationTypeDTO>> GetAllAsync(CancellationToken ct)
    {
        var entities = await _operationTypeRepository.GetAllAsync(ct);

        var models = _mapper.Map<List<OperationTypeDTO>>(entities);
        return models;
    }

    public async Task<OperationTypeDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _operationTypeRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден");
        var model = _mapper.Map<OperationTypeDTO>(entity);
        return model;
    }

    public async Task<List<OperationTypeDTO>> GetAfterDateAsync(string dateTime, CancellationToken ct = default)
    {
        var entities = await _operationTypeRepository.GetAfterDateAsync(dateTime, ct);

        var models = _mapper.Map<List<OperationTypeDTO>>(entities);
        return models;
    }

    public async Task<int> CreateAsync(CreateOperationTypeDTO modelDto, CancellationToken ct = default)
    {
        var entityToCreate = _mapper.Map<OperationType>(modelDto);
        var createdEntity = await _operationTypeRepository.CreateAsync(entityToCreate, ct);
        return createdEntity.Id;
    }

    public async Task UpdateAsync(int id, UpdateOperationTypeDTO modelDto, CancellationToken ct = default)
    {
        var entity = await _operationTypeRepository.GetByIdAsync(id, ct)
            ?? throw new Exception($"Сущность с id {id} не найден.");
        entity = _mapper.Map(modelDto, entity);
        await _operationTypeRepository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default) => await _operationTypeRepository.DeleteByIdAsync(id, ct);
}