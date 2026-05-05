using FurniturePro.Core.Entities;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations;

internal class DeletedIdConfiguration : BaseEntityConfiguration<DeletedId, int>
{
    public override void Configure(EntityTypeBuilder<DeletedId> builder)
    {
        base.Configure(builder);

        builder.ToTable("DeletedIds");

        builder.Property(e => e.TableName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Description)
               .HasColumnType("text");

        builder.Property(e => e.EntityId)
               .IsRequired();

        builder.Property(e => e.ResponsibleEmployeeId)
               .IsRequired();

        builder.HasIndex(e => new { e.TableName, e.EntityId });

        builder.HasOne(e => e.ResponsibleEmployee)
               .WithMany()
               .HasForeignKey(e => e.ResponsibleEmployeeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
