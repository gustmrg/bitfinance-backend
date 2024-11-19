using BitFinance.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Data.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");
            
        builder.Property(b => b.Description)
            .HasColumnName("description")
            .HasColumnType("text")
            .IsRequired();
            
        builder.Property(b => b.Category)
            .HasColumnName("category")
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(b => b.Amount)
            .HasColumnName("amount")
            .HasColumnType("numeric(10,2)")
            .IsRequired();
        
        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestampz")
            .HasPrecision(3)
            .IsRequired();
        
        builder.Property(b => b.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestampz")
            .HasPrecision(3);
        
        builder.Property(b => b.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestampz")
            .HasPrecision(3);
        
        builder.ToTable("expenses");
    }
}