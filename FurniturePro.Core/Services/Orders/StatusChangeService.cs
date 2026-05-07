using AutoMapper;
using FurniturePro.Core.Entities.Orders;
using FurniturePro.Core.Interfaces.Repositories.Orders;
using FurniturePro.Core.Interfaces.Services.Orders;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;
using FurniturePro.Core.Services.Abstractions;

namespace FurniturePro.Core.Services.Orders;

public class StatusChangeService : BaseService<StatusChange, int, StatusChangeDto, CreateStatusChangeDto, UpdateStatusChangeDto>, IStatusChangeService
{
    public StatusChangeService(IStatusChangeRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
