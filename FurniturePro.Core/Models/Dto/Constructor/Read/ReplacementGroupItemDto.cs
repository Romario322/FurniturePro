using FurniturePro.Core.Models.Dto.Abstractions.Read;

namespace FurniturePro.Core.Models.Dto.Constructor.Read;

public class ReplacementGroupItemDto : BaseDto<int>
{
    public required int PartId { get; set; }
    public required int ReplacementGroupId { get; set; }
}