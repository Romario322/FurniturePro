using AutoMapper;
using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;

namespace FurniturePro.Core.Services.System;

public class DeletedIdService : IDeletedIdService
{
    protected readonly IDeletedIdRepository _repository;
    protected readonly IMapper _mapper;

    public DeletedIdService(IDeletedIdRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<DeletedIdDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repository.GetAllAsync(ct);
        return _mapper.Map<List<DeletedIdDto>>(entities);
    }

    public async Task<DeletedIdDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        return entity == null ? default : _mapper.Map<DeletedIdDto>(entity);
    }

    public async Task<List<DeletedIdDto>> GetAfterDateAsync(DateTime dateTime, CancellationToken ct = default)
    {
        var entities = await _repository.GetAfterDateAsync(dateTime, ct);
        return _mapper.Map<List<DeletedIdDto>>(entities);
    }

    public async Task<DeletedIdDto> CreateAsync(CreateDeletedIdDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<DeletedId>(dto);
        var createdEntity = await _repository.CreateAsync(entity, ct);

        return _mapper.Map<DeletedIdDto>(createdEntity);
    }

    public async Task<List<DeletedIdDto>> CreateRangeAsync(List<CreateDeletedIdDto> dtos, CancellationToken ct = default)
    {
        var entities = _mapper.Map<List<DeletedId>>(dtos);
        var createdEntities = await _repository.CreateRangeAsync(entities, ct);

        return _mapper.Map<List<DeletedIdDto>>(createdEntities);
    }

    public async Task UpdateAsync(int id, UpdateDeletedIdDto dto, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ??
            throw new Exception($"Сущность типа {typeof(DeletedId).Name} с id {id} не найдена.");

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _repository.DeleteByIdAsync(id, ct);
    }
}