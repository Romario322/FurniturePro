using FurniturePro.Core.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("materials");

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasKey(et => et.Id);

        builder.HasMany(et => et.Parts)
               .WithOne(e => e.Material)
               .HasForeignKey(e => e.MaterialId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
