using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Models.DTO.Parts;

public class PartDTO
{
    public required int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ColorName { get; set; }
    public int? ColorId { get; set; }
    public string? MaterialName { get; set; }
    public int? MaterialId { get; set; }
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Diameter { get; set; }
    public int? Weight { get; set; }
    public bool Activity { get; set; }
}
