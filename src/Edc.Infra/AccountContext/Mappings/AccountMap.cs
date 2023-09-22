using Edc.Core.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edc.Infra.AccountContext.Mappings;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name)
            .Property(x => x.FullName)
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(128)
            .IsRequired();

        builder.OwnsOne(x => x.Email, email => {
            email.Property(x => x.Address)
                .HasColumnName("Email")
                .HasColumnType("VARCHAR")
                .HasMaxLength(128)
                .IsRequired();
            
            email.OwnsOne(x => x.VerificationCode, verificationCode => {
                verificationCode.Property(x => x.Code)
                    .HasColumnName("EmailVerificationCode")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(6)
                    .IsRequired();
        
                verificationCode.Property(x => x.ExpiresAt)
                    .HasColumnName("EmailVerificationExpiresAt")
                    .IsRequired(false);
                
                verificationCode.Property(x => x.VerifiedAt)
                    .HasColumnName("EmailVerificationVerifiedAt")
                    .IsRequired(false);
                
                verificationCode.Ignore(x => x.IsActive);
            });

            email.HasIndex(x => x.Address)
                .IsUnique();
        });
        
        builder.OwnsOne(x => x.Image)
            .Property(x => x.Url)
            .HasColumnName("ImageUrl")
            .HasColumnType("VARCHAR")
            .HasMaxLength(256)
            .IsRequired(false);

        builder.OwnsOne(x => x.Password)
            .Property(x => x.Hash)
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.OwnsOne(x => x.Password)
            .Property(x => x.ResetCode)
            .HasColumnName("PasswordResetCode")
            .HasColumnType("VARCHAR")
            .HasMaxLength(18)
            .IsRequired();
        
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Accounts)
            .UsingEntity<Dictionary<string, object>>(
                "AccountRole",
                role => role.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                account => account.HasOne<Account>()
                    .WithMany()
                    .HasForeignKey("AccountId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        builder.ToTable("Account");
    }
}