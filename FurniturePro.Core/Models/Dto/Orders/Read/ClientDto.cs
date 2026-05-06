using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Orders.Read;

public class ClientDto : BaseDto<int>
{
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
}