using BitFinance.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Data.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Name).IsRequired();
        
        builder.Property(o => o.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestampz")
            .HasPrecision(3)
            .IsRequired();
        
        builder.Property(o => o.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestampz")
            .HasPrecision(3);
        
        builder.Property(o => o.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestampz")
            .HasPrecision(3);
        
        builder.HasMany(o => o.Bills)
            .WithOne(b => b.Organization)
            .HasForeignKey(b => b.OrganizationId);
            
        builder.ToTable("organizations");
    }
}