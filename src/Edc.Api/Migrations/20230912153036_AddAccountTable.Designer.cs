﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Edc.Api.Migrations
{
    [DbContext(typeof(EdcDbContext))]
    [Migration("20230912153036_AddAccountTable")]
    partial class AddAccountTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Edc.Core.AccountContext.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("Edc.Core.AccountContext.Entities.Account", b =>
                {
                    b.OwnsOne("Edc.Core.SharedContext.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("AccountId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("Email");

                            b1.HasKey("AccountId");

                            b1.HasIndex("Address")
                                .IsUnique();

                            b1.ToTable("Account");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");

                            b1.OwnsOne("Edc.Core.SharedContext.ValueObjects.VerificationCode", "VerificationCode", b2 =>
                                {
                                    b2.Property<Guid>("EmailAccountId")
                                        .HasColumnType("char(36)");

                                    b2.Property<string>("Code")
                                        .IsRequired()
                                        .HasMaxLength(6)
                                        .HasColumnType("VARCHAR")
                                        .HasColumnName("EmailVerificationCode");

                                    b2.Property<DateTime?>("ExpiresAt")
                                        .HasColumnType("datetime(6)")
                                        .HasColumnName("EmailVerificationExpiresAt");

                                    b2.Property<DateTime?>("VerifiedAt")
                                        .HasColumnType("datetime(6)")
                                        .HasColumnName("EmailVerificationVerifiedAt");

                                    b2.HasKey("EmailAccountId");

                                    b2.ToTable("Account");

                                    b2.WithOwner()
                                        .HasForeignKey("EmailAccountId");
                                });

                            b1.Navigation("VerificationCode")
                                .IsRequired();
                        });

                    b.OwnsOne("Edc.Core.SharedContext.ValueObjects.Image", "Image", b1 =>
                        {
                            b1.Property<Guid>("AccountId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Url")
                                .HasMaxLength(256)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("ImageUrl");

                            b1.HasKey("AccountId");

                            b1.ToTable("Account");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");
                        });

                    b.OwnsOne("Edc.Core.SharedContext.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("AccountId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("NVARCHAR")
                                .HasColumnName("Name");

                            b1.HasKey("AccountId");

                            b1.ToTable("Account");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");
                        });

                    b.OwnsOne("Edc.Core.SharedContext.ValueObjects.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("AccountId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Hash")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("PasswordHash");

                            b1.Property<string>("ResetCode")
                                .IsRequired()
                                .HasMaxLength(18)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("PasswordResetCode");

                            b1.HasKey("AccountId");

                            b1.ToTable("Account");

                            b1.WithOwner()
                                .HasForeignKey("AccountId");
                        });

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Password")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
