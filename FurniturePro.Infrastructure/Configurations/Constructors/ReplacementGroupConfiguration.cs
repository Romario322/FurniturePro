using FurniturePro.Core.Entities.Constructors;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Constructors;

internal class ReplacementGroupConfiguration : DictionaryEntityConfiguration<ReplacementGroup, int>
{
    public override void Configure(EntityTypeBuilder<ReplacementGroup> builder)
    {
        base.Configure(builder);

        builder.ToTable("ReplacementGroups");
    }
}