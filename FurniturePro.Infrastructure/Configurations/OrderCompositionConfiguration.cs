using FurniturePro.Core.Entities.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class OrderCompositionConfiguration : IEntityTypeConfiguration<OrderComposition>
{
    public void Configure(EntityTypeBuilder<OrderComposition> builder)
    {
        builder.ToTable("orderCompositions");

        builder.HasKey(et => new { et.Entity1Id, et.Entity2Id });
    }
}
