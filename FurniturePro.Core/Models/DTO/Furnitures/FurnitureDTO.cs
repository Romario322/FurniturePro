using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Models.DTO.Furnitures;

public class FurnitureDTO
{
    public required int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string CategoryName { get; set; }
    public required int CategoryId { get; set; }
    public int? Markup { get; set; }
    public bool Activity { get; set; }
    public List<FurnitureComposition>? FurnitureCompositions { get; set; }
}
