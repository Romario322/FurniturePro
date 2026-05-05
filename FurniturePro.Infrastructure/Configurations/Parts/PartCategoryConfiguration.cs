using FurniturePro.Core.Entities.Parts;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Parts;

internal class PartCategoryConfiguration : DictionaryEntityConfiguration<PartCategory, int>
{
    public override void Configure(EntityTypeBuilder<PartCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("PartCategories");

        builder.HasMany(e => e.Parts)
               .WithOne();
    }
}
