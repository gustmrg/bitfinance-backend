using System.Security.Cryptography;
using BitFinance.API.Services.Interfaces;

namespace BitFinance.API.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly ILogger<LocalFileStorageService> _logger;

    public LocalFileStorageService(
        IConfiguration configuration,
        ILogger<LocalFileStorageService> logger)
    {
        _basePath = configuration["Storage:LocalPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        _logger = logger;
        
        Directory.CreateDirectory(_basePath);
    }
    
    public async Task<FileStorageResult> SaveFileAsync(Stream fileStream, string fileName, string contentType, Guid entityId, string subDirectory = "")
    {
        try
        {
            var relativePath = Path.Combine(
                subDirectory,
                DateTime.UtcNow.Year.ToString(),
                DateTime.UtcNow.Month.ToString("00"),
                entityId.ToString(),
                fileName
            );
            
            var fullPath = Path.Combine(_basePath, relativePath);
            var directory = Path.GetDirectoryName(fullPath)!;
            Directory.CreateDirectory(directory);
            
            using var sha256 = SHA256.Create();
            using var fileOutputStream = new FileStream(fullPath, FileMode.Create);
            
            var buffer = new byte[8192];
            int bytesRead;
            long totalBytes = 0;
            
            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileOutputStream.WriteAsync(buffer, 0, bytesRead);
                sha256.TransformBlock(buffer, 0, bytesRead, null, 0);
                totalBytes += bytesRead;
            }
            
            sha256.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            var hash = Convert.ToBase64String(sha256.Hash!);
            
            return new FileStorageResult
            {
                Success = true,
                StoragePath = relativePath.Replace('\\', '/'), // Normalize path separators
                FileName = fileName,
                FileSizeInBytes = totalBytes,
                FileHash = hash
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file {FileName}", fileName);
            return new FileStorageResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<Stream> GetFileAsync(string storagePath)
    {
        var fullPath = Path.Combine(_basePath, storagePath);
        
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"File not found: {storagePath}");
            
        return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
    }

    public async Task<bool> DeleteFileAsync(string storagePath)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, storagePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {StoragePath}", storagePath);
            return false;
        }
    }

    public Task<bool> FileExistsAsync(string storagePath)
    {
        throw new NotImplementedException();
    }

    public string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var guid = Guid.NewGuid().ToString("N").Substring(0, 8);
        
        return $"{fileNameWithoutExtension}_{timestamp}_{guid}{extension}";
    }
}