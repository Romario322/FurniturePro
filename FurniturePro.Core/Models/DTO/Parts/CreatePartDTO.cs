namespace FurniturePro.Core.Models.DTO.Parts;

public class CreatePartDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int? ColorId { get; set; }
    public int? MaterialId { get; set; }
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Diameter { get; set; }
    public decimal? Weight { get; set; }
    public required bool Activity { get; set; }
}
