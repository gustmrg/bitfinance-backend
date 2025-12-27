using BitFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Token)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasPrecision(3)
            .IsRequired();

        builder.Property(r => r.ExpiresAt)
            .HasColumnType("timestamp with time zone")
            .HasPrecision(3)
            .IsRequired();

        builder.Property(r => r.RevokedAt)
            .HasColumnType("timestamp with time zone")
            .HasPrecision(3);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => r.Token)
            .IsUnique();

        builder.HasIndex(r => r.UserId);

        builder.Ignore(r => r.IsExpired);
        builder.Ignore(r => r.IsRevoked);
        builder.Ignore(r => r.IsActive);

        builder.ToTable("refresh_tokens");
    }
}
