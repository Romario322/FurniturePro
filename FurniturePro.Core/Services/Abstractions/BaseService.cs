using AutoMapper;
using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Interfaces.Repositories.Abstractions;
using FurniturePro.Core.Interfaces.Services.Abstractions;

namespace FurniturePro.Core.Services.Abstractions;

public abstract class BaseService<TEntity, TId, TReadDto, TCreateDto, TUpdateDto>
    : IBaseService<TId, TReadDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TId : notnull
{
    protected readonly IBaseRepository<TEntity, TId> _repository;
    protected readonly IMapper _mapper;

    protected BaseService(IBaseRepository<TEntity, TId> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<List<TReadDto>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _repository.GetAllAsync(ct);
        return _mapper.Map<List<TReadDto>>(entities);
    }

    public virtual async Task<TReadDto?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        return entity == null ? default : _mapper.Map<TReadDto>(entity);
    }

    public virtual async Task<List<TReadDto>> GetAfterDateAsync(DateTime dateTime, CancellationToken ct = default)
    {
        var entities = await _repository.GetAfterDateAsync(dateTime, ct);
        return _mapper.Map<List<TReadDto>>(entities);
    }

    public virtual async Task<TReadDto> CreateAsync(TCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var createdEntity = await _repository.CreateAsync(entity, ct);

        return _mapper.Map<TReadDto>(createdEntity);
    }

    public virtual async Task<List<TReadDto>> CreateRangeAsync(List<TCreateDto> dtos, CancellationToken ct = default)
    {
        var entities = _mapper.Map<List<TEntity>>(dtos);
        var createdEntities = await _repository.CreateRangeAsync(entities, ct);

        return _mapper.Map<List<TReadDto>>(createdEntities);
    }

    public virtual async Task UpdateAsync(TId id, TUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct) ??
            throw new Exception($"Сущность типа {typeof(TEntity).Name} с id {id} не найдена.");

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity, ct);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken ct = default)
    {
        await _repository.DeleteByIdAsync(id, ct);
    }
}