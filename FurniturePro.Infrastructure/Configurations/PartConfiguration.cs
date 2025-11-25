using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.ToTable("parts");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.Prices)
               .WithOne(e => e.Part)
               .HasForeignKey(e => e.PartId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(et => et.Counts)
               .WithOne(e => e.Part)
               .HasForeignKey(e => e.PartId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
