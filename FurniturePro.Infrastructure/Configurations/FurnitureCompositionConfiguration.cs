using FurniturePro.Core.Entities.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class FurnitureCompositionConfiguration : IEntityTypeConfiguration<FurnitureComposition>
{
    public void Configure(EntityTypeBuilder<FurnitureComposition> builder)
    {
        builder.ToTable("furnitureCompositions");

        builder.HasKey(et => new { et.Entity1Id, et.Entity2Id });
    }
}
