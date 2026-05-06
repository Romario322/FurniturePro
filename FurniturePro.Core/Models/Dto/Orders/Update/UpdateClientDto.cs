using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Orders.Update;

public class UpdateClientDto : UpdateBaseDto<int>
{
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
}