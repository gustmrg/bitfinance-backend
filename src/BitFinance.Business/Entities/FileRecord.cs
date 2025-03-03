using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

public class FileRecord
{
    public FileRecord(string fileName, string storagePath, StorageProvider storageProvider)
    {
        Id = Guid.NewGuid(); 
        CreatedAt = DateTime.UtcNow;
        FileName = fileName;
        StoragePath = storagePath;
        StorageProvider = storageProvider;
    }
    
    
    
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string StoragePath { get; set; }
    public long Size { get; set; } 
    public string ContentType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public StorageProvider StorageProvider { get; set; }
    public string? Metadata { get; set; }
}
