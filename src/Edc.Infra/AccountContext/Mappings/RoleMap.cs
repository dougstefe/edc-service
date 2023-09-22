using Edc.Core.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edc.Infra.AccountContext.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnType("VARCHAR")
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("VARCHAR")
            .HasMaxLength(128)
            .IsRequired();
        
        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.ToTable("Role");
    }
}