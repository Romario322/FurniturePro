using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Constructor.Update;

public class UpdateReplacementGroupItemDto
{
    public required int PartId { get; set; }
    public required int ReplacementGroupId { get; set; }
}