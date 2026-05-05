using FurniturePro.Core.Entities.Users;
using FurniturePro.Infrastructure.Configurations.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurniturePro.Infrastructure.Configurations.Users;

internal class EmployeeConfiguration : BaseEntityConfiguration<Employee, int>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);

        builder.ToTable("Employees");

        builder.Property(e => e.FullName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Login)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(e => e.Login)
               .IsUnique();

        builder.Property(e => e.HashPassword)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.Activity).IsRequired();

        builder.HasOne(e => e.SystemRole)
               .WithMany(r => r.Employees)
               .HasForeignKey(e => e.SystemRoleId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}