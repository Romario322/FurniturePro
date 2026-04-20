using FurniturePro.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class DeletedIdConfiguration : IEntityTypeConfiguration<DeletedId>
{
    public void Configure(EntityTypeBuilder<DeletedId> builder)
    {
        builder.ToTable("deletedIds");

        builder.HasIndex(c => new { c.TableName, c.EntityId }).IsUnique();

        builder.HasKey(et => et.Id);
    }
}
