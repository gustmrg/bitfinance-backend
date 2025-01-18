﻿// <auto-generated />
using System;
using BitFinance.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BitFinance.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250118023053_AddOrganizationInvite")]
    partial class AddOrganizationInvite
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BitFinance.Business.Entities.Bill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("AmountDue")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("amount_due");

                    b.Property<decimal?>("AmountPaid")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("amount_paid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime>("DueDate")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("due_date");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("organization_id");

                    b.Property<DateTime?>("PaymentDate")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("payment_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_bills");

                    b.HasIndex("OrganizationId")
                        .HasDatabaseName("ix_bills_organization_id");

                    b.ToTable("bills", (string)null);
                });

            modelBuilder.Entity("BitFinance.Business.Entities.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("amount");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("organization_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_expenses");

                    b.HasIndex("OrganizationId")
                        .HasDatabaseName("ix_expenses_organization_id");

                    b.ToTable("expenses", (string)null);
                });

            modelBuilder.Entity("BitFinance.Business.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasPrecision(3)
                        .HasColumnType("timestampz")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_organizations");

                    b.ToTable("organizations", (string)null);
                });

            modelBuilder.Entity("BitFinance.Business.Entities.OrganizationInvite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_used");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("organization_id");

                    b.HasKey("Id")
                        .HasName("pk_organization_invites");

                    b.ToTable("organization_invites");
                });

            modelBuilder.Entity("BitFinance.Business.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("email_index");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("user_name_index");

                    b.ToTable("asp_net_users", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("role_name_index");

                    b.ToTable("asp_net_roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_role_claims_role_id");

                    b.ToTable("asp_net_role_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_claims_user_id");

                    b.ToTable("asp_net_user_claims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_asp_net_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_logins_user_id");

                    b.ToTable("asp_net_user_logins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("RoleId")
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_asp_net_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_user_roles_role_id");

                    b.ToTable("asp_net_user_roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_asp_net_user_tokens");

                    b.ToTable("asp_net_user_tokens", (string)null);
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.Property<string>("MembersId")
                        .HasColumnType("text")
                        .HasColumnName("members_id");

                    b.Property<Guid>("OrganizationsId")
                        .HasColumnType("uuid")
                        .HasColumnName("organizations_id");

                    b.HasKey("MembersId", "OrganizationsId")
                        .HasName("pk_organization_user");

                    b.HasIndex("OrganizationsId")
                        .HasDatabaseName("ix_organization_user_organizations_id");

                    b.ToTable("organization_user");
                });

            modelBuilder.Entity("BitFinance.Business.Entities.Bill", b =>
                {
                    b.HasOne("BitFinance.Business.Entities.Organization", "Organization")
                        .WithMany("Bills")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bills_organizations_organization_id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("BitFinance.Business.Entities.Expense", b =>
                {
                    b.HasOne("BitFinance.Business.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_expenses_organizations_organization_id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_role_claims_asp_net_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BitFinance.Business.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_claims_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BitFinance.Business.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_logins_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_roles_role_id");

                    b.HasOne("BitFinance.Business.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BitFinance.Business.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_tokens_asp_net_users_user_id");
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.HasOne("BitFinance.Business.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_organization_user_asp_net_users_members_id");

                    b.HasOne("BitFinance.Business.Entities.Organization", null)
                        .WithMany()
                        .HasForeignKey("OrganizationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_organization_user_organizations_organizations_id");
                });

            modelBuilder.Entity("BitFinance.Business.Entities.Organization", b =>
                {
                    b.Navigation("Bills");
                });
#pragma warning restore 612, 618
        }
    }
}
