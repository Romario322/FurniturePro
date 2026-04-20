namespace FurniturePro.Core.Models.DTO.OperationTypes;

public class OperationTypeDTO
{
    public required int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime UpdateDate { get; set; }
}
