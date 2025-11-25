using FurniturePro.Core.Entities;
using FurniturePro.Core.Entities.Connections;

namespace FurniturePro.Core.Models.DTO.Furnitures;

public class UpdateFurnitureDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required int CategoryId { get; set; }
    public int? Markup { get; set; }
    public bool Activity { get; set; }
}
