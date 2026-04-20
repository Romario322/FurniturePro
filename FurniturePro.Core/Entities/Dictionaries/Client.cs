using FurniturePro.Core.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniturePro.Core.Entities.Dictionaries;

public class Client : BaseEntity<int>
{
    [Column(TypeName = "varchar(200)")]
    public required string FullName { get; set; }
    [Column(TypeName = "varchar(200)")]
    public required string Phone { get; set; }
    [Column(TypeName = "varchar(200)")]
    public required string Email { get; set; }

    public List<Order>? Orders { get; set; }
}
