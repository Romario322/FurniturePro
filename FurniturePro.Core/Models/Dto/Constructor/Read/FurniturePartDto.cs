using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Constructor.Read;

public class FurniturePartDto : BaseDto<int>
{
    public required int Quantity { get; set; }

    public required int FurnitureId { get; set; }
    public required int PartRoleId { get; set; }
    public required int ReplacementGroupId { get; set; }
}