using FurniturePro.Core.Entities.Abstractions;
using FurniturePro.Core.Entities.Constructor;
using FurniturePro.Core.Entities.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Parts;

public class Part : DictionaryEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string Sku { get; set; }

    [Column(TypeName = "integer")]
    public int? Thickness { get; set; }

    [Column(TypeName = "integer")]
    public int? WasteCoefficient { get; set; }

    [Column(TypeName = "decimal(10, 3)")]
    public required decimal WeightPerUnit { get; set; }

    [Column(TypeName = "boolean")]
    public bool Activity { get; set; }

    [Column(TypeName = "integer")]
    public required int MaterialId { get; set; }
    public Material? Material { get; set; }

    [Column(TypeName = "integer")]
    public required int ColorId { get; set; }
    public Color? Color { get; set; }

    [Column(TypeName = "integer")]
    public required int PartTypeId { get; set; }
    public PartType? PartType { get; set; }

    [Column(TypeName = "integer")]
    public required int PartCategoryId { get; set; }
    public PartCategory? PartCategory { get; set; }

    public List<ReplacementGroupItem>? ReplacementGroupItems { get; set; }
    public List<Price>? Prices { get; set; }
    public List<OrderPartDetail>? OrderPartDetails { get; set; }
}