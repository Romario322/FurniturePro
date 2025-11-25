namespace FurniturePro.Core.Models.DTO.Categories;

public class CategoryDTO
{
    public required int Id { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
}
