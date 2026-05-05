using FurniturePro.Core.Entities.FurnitureEntities;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.FurnitureConfigurations;

internal class FurnitureConfiguration : CatalogEntityConfiguration<Furniture, int>
{
    public override void Configure(EntityTypeBuilder<Furniture> builder)
    {
        base.Configure(builder);

        builder.ToTable("Furnitures");

        builder.Property(e => e.BaseWidth).IsRequired();
        builder.Property(e => e.BaseHeight).IsRequired();
        builder.Property(e => e.BaseDepth).IsRequired();
        builder.Property(e => e.Markup).IsRequired();
        builder.Property(e => e.Activity).IsRequired();

        builder.HasOne(e => e.FurnitureCategory)
               .WithMany(c => c.Furnitures)
               .HasForeignKey(e => e.FurnitureCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
