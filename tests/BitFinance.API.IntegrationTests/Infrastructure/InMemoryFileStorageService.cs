using BitFinance.API.Services.Interfaces;

namespace BitFinance.API.IntegrationTests.Infrastructure;

public class InMemoryFileStorageService : IFileStorageService
{
    private readonly Dictionary<string, byte[]> _files = new();

    public async Task<FileStorageResult> SaveFileAsync(Stream fileStream, string fileName, string contentType, Guid entityId, string subDirectory = "")
    {
        var uniqueName = GenerateUniqueFileName(fileName);
        var storagePath = string.IsNullOrEmpty(subDirectory)
            ? $"{entityId}/{uniqueName}"
            : $"{subDirectory}/{entityId}/{uniqueName}";

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        var bytes = ms.ToArray();
        _files[storagePath] = bytes;

        return new FileStorageResult
        {
            Success = true,
            StoragePath = storagePath,
            FileName = uniqueName,
            FileSizeInBytes = bytes.Length,
            FileHash = "testhash"
        };
    }

    public Task<Stream> GetFileAsync(string storagePath)
    {
        if (!_files.TryGetValue(storagePath, out var bytes))
            throw new FileNotFoundException($"File not found: {storagePath}");

        return Task.FromResult<Stream>(new MemoryStream(bytes));
    }

    public Task<bool> DeleteFileAsync(string storagePath)
    {
        return Task.FromResult(_files.Remove(storagePath));
    }

    public Task<bool> FileExistsAsync(string storagePath)
    {
        return Task.FromResult(_files.ContainsKey(storagePath));
    }

    public string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        return $"{Guid.NewGuid()}{extension}";
    }
}
