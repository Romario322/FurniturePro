using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Furniture;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Constructor;

public class FurniturePart : BaseEntity<int>
{

    [Column(TypeName = "integer")]
    public required int Quantity { get; set; }


    [Column(TypeName = "integer")]
    public required int FurnitureId { get; set; }
    public Furniture.Furniture? Furniture { get; set; }

    [Column(TypeName = "integer")]
    public required int PartRoleId { get; set; }
    public PartRole? PartRole { get; set; }

    [Column(TypeName = "integer")]
    public required int ReplacementGroupId { get; set; }
    public ReplacementGroup? ReplacementGroup { get; set; }
}
