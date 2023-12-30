using BitFinance.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Data.Mappings;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id);
        
        builder.Property(b => b.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(b => b.Category)
            .IsRequired();
        
        builder.Property(b => b.CreatedDate)
            .HasColumnType("datetime2")
            .HasPrecision(3)
            .IsRequired();
        
        builder.Property(b => b.DueDate)
            .HasColumnType("datetime2")
            .HasPrecision(3)
            .IsRequired();
        
        builder.Property(b => b.PaidDate)
            .HasColumnType("datetime2")
            .HasPrecision(3)
            .IsRequired(false);
        
        builder.Property(b => b.AmountDue)
            .HasColumnType("decimal(10,2)")
            .IsRequired();
        
        builder.Property(b => b.AmountPaid)
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);
        
        builder.Property(b => b.IsPaid)
            .IsRequired();
        
        builder.Property(b => b.IsDeleted)
            .IsRequired();

        builder.ToTable("Bills");
    }
}