using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Constructor;

public class ReplacementGroupItem : BaseEntity<int>
{
    [Column(TypeName = "integer")]
    public required int PartId { get; set; }
    public Part? Part { get; set; }

    [Column(TypeName = "integer")]
    public required int ReplacementGroupId { get; set; }
    public ReplacementGroup? ReplacementGroup { get; set; }
}
