using FurniturePro.Core.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Abstractions;

internal abstract class CatalogEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : CatalogEntity<TId>
    where TId : notnull
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Sku)
            .IsUnique();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.UpdateDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(e => e.UpdateDate);
    }
}
