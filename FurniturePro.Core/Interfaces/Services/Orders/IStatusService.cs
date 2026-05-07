using FurniturePro.Core.Enums;
using FurniturePro.Core.Interfaces.Services.Abstractions;
using FurniturePro.Core.Models.Dto.Orders.Create;
using FurniturePro.Core.Models.Dto.Orders.Read;
using FurniturePro.Core.Models.Dto.Orders.Update;

namespace FurniturePro.Core.Interfaces.Services.Orders;

public interface IStatusService : IBaseService<StatusEnum, StatusDto, CreateStatusDto, UpdateStatusDto>
{
}