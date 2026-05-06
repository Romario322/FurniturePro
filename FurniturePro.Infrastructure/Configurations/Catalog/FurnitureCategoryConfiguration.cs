using FurniturePro.Core.Entities.Catalog;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Catalog;

internal class FurnitureCategoryConfiguration : DictionaryEntityConfiguration<FurnitureCategory, int>
{
    public override void Configure(EntityTypeBuilder<FurnitureCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("FurnitureCategories");

        builder.HasMany(e => e.Furnitures)
               .WithOne(f => f.FurnitureCategory)
               .HasForeignKey(f => f.FurnitureCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
