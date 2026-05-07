using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.System.Update;

public class UpdateEmployeeDto
{
    public required string FullName { get; set; }
    public required string Login { get; set; }
    public required string HashPassword { get; set; }
    public bool Activity { get; set; }

    public required int SystemRoleId { get; set; }
}