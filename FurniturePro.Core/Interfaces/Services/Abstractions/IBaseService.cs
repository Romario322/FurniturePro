namespace FurniturePro.Core.Interfaces.Services.Abstractions;

public interface IBaseService<TId, TReadDto, TCreateDto, TUpdateDto> where TId : notnull
{
    Task<List<TReadDto>> GetAllAsync(CancellationToken ct = default);

    Task<TReadDto?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task<List<TReadDto>> GetAfterDateAsync(DateTime dateTime, CancellationToken ct = default);

    Task<TReadDto> CreateAsync(TCreateDto dto, CancellationToken ct = default);

    Task<List<TReadDto>> CreateRangeAsync(List<TCreateDto> dtos, CancellationToken ct = default);

    Task UpdateAsync(TId id, TUpdateDto dto, CancellationToken ct = default);

    Task DeleteAsync(TId id, CancellationToken ct = default);
}