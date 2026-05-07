using AutoMapper;
using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Interfaces.Repositories.Constructor;
using FurniturePro.Core.Interfaces.Services.Constructor;
using FurniturePro.Core.Models.Dto.Constructor.Create;
using FurniturePro.Core.Models.Dto.Constructor.Read;
using FurniturePro.Core.Models.Dto.Constructor.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Constructor;

public class ReplacementGroupService : BaseService<ReplacementGroup, int, ReplacementGroupDto, CreateReplacementGroupDto, UpdateReplacementGroupDto>, IReplacementGroupService
{
    public ReplacementGroupService(IReplacementGroupRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
