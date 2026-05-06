using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Core.Enums;

namespace FurniturePro.Core.Entities.Constructor;

public class FurniturePart : BaseEntity<int>
{
    public required int Quantity { get; set; }

    public required int FurnitureId { get; set; }
    public Furniture? Furniture { get; set; }
    public required PartRoleEnum PartRoleId { get; set; }
    public PartRole? PartRole { get; set; }
    public required int ReplacementGroupId { get; set; }
    public ReplacementGroup? ReplacementGroup { get; set; }
}
