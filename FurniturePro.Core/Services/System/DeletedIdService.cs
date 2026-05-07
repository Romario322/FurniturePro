using AutoMapper;
using FurniturePro.Core.Entities.System;
using FurniturePro.Core.Interfaces.Repositories.System;
using FurniturePro.Core.Interfaces.Services.System;
using FurniturePro.Core.Models.Dto.System.Create;
using FurniturePro.Core.Models.Dto.System.Read;
using FurniturePro.Core.Models.Dto.System.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.System;

public class DeletedIdService : BaseService<DeletedId, int, DeletedIdDto, CreateDeletedIdDto, UpdateDeletedIdDto>, IDeletedIdService
{
    public DeletedIdService(IDeletedIdRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
