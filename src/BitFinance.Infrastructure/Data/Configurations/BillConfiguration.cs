using BitFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Infrastructure.Data.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("bill_id");
        
        builder.Property(b => b.Name)
            .HasColumnType("varchar(255)")
            .HasColumnName("name")
            .IsRequired();
        
        builder.Property(b => b.Category)
            .HasColumnType("varchar(255)")
            .HasColumnName("category")
            .IsRequired();
        
        builder.Property(b => b.CreatedDate)
            .HasColumnType("timestamp")
            .HasColumnName("created_date")
            .IsRequired();
        
        builder.Property(b => b.DueDate)
            .HasColumnType("timestamp")
            .HasColumnName("due_date")
            .IsRequired();
        
        builder.Property(b => b.PaidDate)
            .HasColumnType("timestamp")
            .HasColumnName("paid_date")
            .IsRequired(false);
        
        builder.Property(b => b.AmountDue)
            .HasColumnName("amount_due")
            .HasPrecision(10,2)
            .IsRequired();
        
        builder.Property(b => b.AmountPaid)
            .HasColumnName("amount_paid")
            .HasPrecision(10,2)
            .IsRequired(false);
        
        builder.Property(b => b.IsPaid)
            .HasColumnName("is_paid")
            .IsRequired();
        
        builder.Property(b => b.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired(false);

        builder.ToTable("bills");
    }
}