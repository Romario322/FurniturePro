using FurniturePro.Core.Models.Dto.Abstractions.Update;

namespace FurniturePro.Core.Models.Dto.Constructor.Update;

public class UpdateReplacementGroupItemDto : UpdateBaseDto<int>
{
    public required int PartId { get; set; }
    public required int ReplacementGroupId { get; set; }
}