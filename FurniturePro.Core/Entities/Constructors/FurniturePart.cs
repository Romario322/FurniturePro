using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.FurnitureEntities;

namespace FurniturePro.Core.Entities.Constructors;

public class FurniturePart : BaseEntity<int>
{
    public required int Quantity { get; set; }

    public required int FurnitureId { get; set; }
    public Furniture? Furniture { get; set; }
    public required int PartRoleId { get; set; }
    public PartRole? PartRole { get; set; }
    public required int ReplacementGroupId { get; set; }
    public ReplacementGroup? ReplacementGroup { get; set; }
}
