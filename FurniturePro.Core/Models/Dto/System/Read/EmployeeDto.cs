using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.System.Read;

public class EmployeeDto : BaseDto<int>
{
    public required string FullName { get; set; }
    public required string Login { get; set; }
    public required string HashPassword { get; set; }
    public bool Activity { get; set; }

    public required int SystemRoleId { get; set; }
}