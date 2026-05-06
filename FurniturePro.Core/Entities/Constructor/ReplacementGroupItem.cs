using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Parts;

namespace FurniturePro.Core.Entities.Constructor;

public class ReplacementGroupItem : BaseEntity<int>
{
    public required int PartId { get; set; }
    public Part? Part { get; set; }
    public required int ReplacementGroupId { get; set; }
    public ReplacementGroup? ReplacementGroup { get; set; }
}
