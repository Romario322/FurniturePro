using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class CountConfiguration : IEntityTypeConfiguration<Count>
{
    public void Configure(EntityTypeBuilder<Count> builder)
    {
        builder.ToTable("counts");

        builder.HasKey(et => et.Id);
    }
}
