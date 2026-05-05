using FurniturePro.Core.Entities.Parts;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Parts;

internal class PartConfiguration : CatalogEntityConfiguration<Part, int>
{
    public override void Configure(EntityTypeBuilder<Part> builder)
    {
        base.Configure(builder);

        builder.ToTable("Parts");

        builder.Property(e => e.Thickness);
        builder.Property(e => e.WasteCoefficient);
        builder.Property(e => e.Activity).IsRequired();

        builder.Property(e => e.WeightPerUnit)
               .IsRequired()
               .HasColumnType("numeric(10, 3)");

        builder.HasOne(e => e.Material)
               .WithMany(m => m.Parts)
               .HasForeignKey(e => e.MaterialId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Color)
               .WithMany(c => c.Parts)
               .HasForeignKey(e => e.ColorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PartType)
               .WithMany(pt => pt.Parts)
               .HasForeignKey(e => e.PartTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PartCategory)
               .WithMany(pc => pc.Parts)
               .HasForeignKey(e => e.PartCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
