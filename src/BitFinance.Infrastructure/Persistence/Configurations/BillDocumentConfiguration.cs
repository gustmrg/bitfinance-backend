using BitFinance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BitFinance.Infrastructure.Persistence.Configurations;

public class BillDocumentConfiguration : IEntityTypeConfiguration<BillDocument>
{
    public void Configure(EntityTypeBuilder<BillDocument> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FileName)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(d => d.OriginalFileName)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(d => d.ContentType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.FileSizeInBytes)
            .IsRequired();

        builder.Property(d => d.StoragePath)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(d => d.DocumentType)
            .HasConversion<string>()
            .HasColumnType("text")
            .IsRequired();

        builder.Property(d => d.StorageProvider)
            .HasConversion<string>()
            .HasColumnType("text")
            .IsRequired();

        builder.Property(d => d.Description)
            .HasColumnType("text");

        builder.Property(d => d.FileHash)
            .HasMaxLength(64);

        builder.Property(d => d.UploadedAt)
            .HasColumnType("timestamptz")
            .HasPrecision(3)
            .IsRequired();

        builder.Property(d => d.DeletedAt)
            .HasColumnType("timestamptz")
            .HasPrecision(3);

        builder.HasOne(d => d.Bill)
            .WithMany(b => b.Documents)
            .HasForeignKey(d => d.BillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => d.BillId);

        builder.HasQueryFilter(d => d.DeletedAt == null);

        builder.ToTable("bill_documents");
    }
}
