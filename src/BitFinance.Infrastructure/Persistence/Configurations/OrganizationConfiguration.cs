using BitFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Name).IsRequired();
        
        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .HasPrecision(3)
            .IsRequired();
        
        builder.Property(o => o.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .HasPrecision(3);
        
        builder.Property(o => o.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestamp with time zone")
            .HasPrecision(3);

        builder.Property(o => o.TimeZoneId)
            .HasColumnName("timezone_id")
            .HasMaxLength(100)
            .HasDefaultValue("America/Sao_Paulo")
            .IsRequired();

        builder.HasMany(o => o.Bills)
            .WithOne(b => b.Organization)
            .HasForeignKey(b => b.OrganizationId);

        builder.HasQueryFilter(o => o.DeletedAt == null);

        builder.ToTable("organizations");
    }
}