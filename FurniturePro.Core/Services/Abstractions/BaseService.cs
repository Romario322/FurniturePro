using AutoMapper;
using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.Abstractions;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;

namespace FurniturePro.Core.Services.Abstractions;

public abstract class BaseService<TEntity, TId, TReadDto, TCreateDto, TUpdateDto>
    : IBaseService<TId, TReadDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TId : notnull
{
    protected readonly IBaseRepository<TEntity, TId> _repository; 
    protected readonly ICurrentUserService _currentUserService;
    protected readonly IDeletedIdRepository _deletedIdRepository;
    protected readonly IMapper _mapper;

    protected BaseService(IBaseRepository<TEntity, TId> repository, ICurrentUserService currentUserService, IDeletedIdRepository deletedIdRepository, IMapper mapper)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _deletedIdRepository = deletedIdRepository;
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
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity == null) return;

        var currentUserId = _currentUserService.GetUserId();

        var deletedIdDto = new CreateDeletedIdDto
        {
            EntityId = id.ToString(),
            Description = $"Удалена запись: {entity.ToString()}",
            ResponsibleEmployeeId = currentUserId ?? 0,
            TableName = typeof(TEntity).Name,
            DeletedAt = DateTime.UtcNow
        };

        var deletedId = _mapper.Map<DeletedId>(deletedIdDto);

        await _deletedIdRepository.CreateAsync(deletedId, ct);

        await _repository.DeleteByIdAsync(id, ct);
    }
}