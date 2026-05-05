using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Constructor;

public class ReplacementGroup : DictionaryEntity<int>
{
    public List<ReplacementGroupItem>? ReplacementGroupItems { get; set; }
    public List<FurniturePart>? FurnitureParts { get; set; }
}
