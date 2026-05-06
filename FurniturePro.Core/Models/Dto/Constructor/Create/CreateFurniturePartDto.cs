namespace FurniturePro.Core.Models.Dto.Constructor.Create;

public class CreateFurniturePartDto
{
    public required int Quantity { get; set; }

    public required int FurnitureId { get; set; }
    public required int PartRoleId { get; set; }
    public required int ReplacementGroupId { get; set; }
}