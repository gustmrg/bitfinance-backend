using BitFinance.Business.Interfaces;

namespace BitFinance.Data.Storage;

public class LocalStorageProvider : IDocumentStorageProvider
{
    private readonly string _basePath;
    
    public LocalStorageProvider(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }
    
    public async Task SaveAsync(string path, byte[] fileData)
    {
        string fullPath = Path.Combine(_basePath, path);
        string? directory = Path.GetDirectoryName(fullPath);

        // Step 1: Ensure the directory exists
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Step 2: Save the file
        await File.WriteAllBytesAsync(fullPath, fileData);
    }

    public async Task<byte[]> LoadAsync(string path)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string path)
    {
        throw new NotImplementedException();
    }
}