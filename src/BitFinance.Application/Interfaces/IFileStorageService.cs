namespace BitFinance.Application.Interfaces;

public interface IFileStorageService
{
    Task<FileStorageResult> SaveFileAsync(Stream fileStream, string fileName, string contentType, Guid entityId, string subDirectory = "");
    Task<Stream> GetFileAsync(string storagePath);
    Task<bool> DeleteFileAsync(string storagePath);
    Task<bool> FileExistsAsync(string storagePath);
    string GenerateUniqueFileName(string originalFileName);
}

public class FileStorageResult
{
    public bool Success { get; set; }
    public string? StoragePath { get; set; }
    public string? FileName { get; set; }
    public string? ErrorMessage { get; set; }
    public long? FileSizeInBytes { get; set; }
    public string? FileHash { get; set; }
}