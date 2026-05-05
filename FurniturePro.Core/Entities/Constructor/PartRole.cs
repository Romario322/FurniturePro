using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Constructor;

public class PartRole : DictionaryEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string LengthFormula { get; set; }

    [Column(TypeName = "varchar(200)")]
    public required string WidthFormula { get; set; }

    public List<ReplacementGroup>? ReplacementGroups { get; set; }
}
