using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;
using System.Security.Cryptography;

namespace FurniturePro.Core.Interfaces.Services.System;

public interface IDeletedIdService
{
    Task<List<DeletedIdDto>> GetAllAsync(CancellationToken ct = default);

    Task<DeletedIdDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<DeletedIdDto>> GetAfterDateAsync(DateTime dateTime, CancellationToken ct = default);

    Task<DeletedIdDto> CreateAsync(CreateDeletedIdDto dto, CancellationToken ct = default);

    Task<List<DeletedIdDto>> CreateRangeAsync(List<CreateDeletedIdDto> dtos, CancellationToken ct = default);

    Task UpdateAsync(int id, UpdateDeletedIdDto dto, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}