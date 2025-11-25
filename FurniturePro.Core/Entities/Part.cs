using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities;

public class Part : DictionaryEntity<int>
{
    [Column(TypeName = "integer")]
    public int? Length { get; set; }
    [Column(TypeName = "integer")]
    public int? Width { get; set; }
    [Column(TypeName = "integer")]
    public int? Height { get; set; }
    [Column(TypeName = "integer")]
    public int? Diameter { get; set; }

    [Column(TypeName = "integer")]
    public int? Weight { get; set; }

    [Column(TypeName = "boolean")]
    public bool Activity { get; set; }

    public List<Price>? Prices { get; set; }

    public List<Count>? Counts { get; set; }

    [Column(TypeName = "integer")]
    public int? ColorId { get; set; }
    public Color? Color { get; set; }

    [Column(TypeName = "integer")]
    public int? MaterialId { get; set; }
    public Material? Material { get; set; }
}
