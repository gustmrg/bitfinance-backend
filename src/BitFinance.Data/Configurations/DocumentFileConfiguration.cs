using BitFinance.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Data.Configurations;

public class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
{
    public void Configure(EntityTypeBuilder<DocumentFile> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FileName)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.FileType)
            .HasMaxLength(3)
            .IsRequired();
        
        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestampz")
            .HasPrecision(3)
            .IsRequired();
        
        builder.ToTable("document_files");
    }
}