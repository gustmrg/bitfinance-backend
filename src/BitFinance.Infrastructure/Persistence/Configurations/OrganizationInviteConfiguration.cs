using BitFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Infrastructure.Persistence.Configurations;

public class OrganizationInviteConfiguration : IEntityTypeConfiguration<OrganizationInvite>
{
    public void Configure(EntityTypeBuilder<OrganizationInvite> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CreatedAt)
            .HasColumnType("timestamptz")
            .HasPrecision(3)
            .IsRequired();

        builder.Property(i => i.ExpiresAt)
            .HasColumnType("timestamptz")
            .HasPrecision(3)
            .IsRequired();

        builder.Property(i => i.IsUsed)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(i => i.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.OrganizationId);

        builder.ToTable("organization_invites");
    }
}
