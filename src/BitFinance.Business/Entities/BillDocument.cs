using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

public class BillDocument
{
    public Guid Id { get; set; }
    public Guid BillId { get; set; }
    public Bill Bill { get; set; } = null!;
    
    // File metadata
    public string FileName { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long FileSizeInBytes { get; set; }
    
    // Storage information
    public string StoragePath { get; set; } = null!; 
    public DocumentType DocumentType { get; set; }  
    public StorageProvider StorageProvider { get; set; } 
    
    // Optional metadata
    public string? Description { get; set; }
    public string? FileHash { get; set; }  // SHA256 for integrity
    
    // Audit fields
    public DateTime UploadedAt { get; set; }
    public Guid? UploadedByUserId { get; set; }
    public DateTime? DeletedAt { get; set; }
}