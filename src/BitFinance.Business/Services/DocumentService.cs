using BitFinance.Business.Interfaces;

namespace BitFinance.Business.Services;

public class DocumentService
{
    private readonly IDocumentStorageProvider _storageProvider;

    public DocumentService(IDocumentStorageProvider documentStorageProvider)
    {
        _storageProvider = documentStorageProvider;
    }
    
    public async Task<string> SaveDocumentAsync(Guid paymentId, string originalFileName, byte[] fileData)
    {
        if (fileData == null || fileData.Length == 0)
            throw new ArgumentException("File data cannot be empty.");

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new ArgumentException("File name is required.");
        
        
        string uniqueFileName = GetUniqueFileName(originalFileName);
        
        string directory = $"{paymentId}";
        string fullPath = Path.Combine(directory, uniqueFileName);
        
        await _storageProvider.SaveAsync(fullPath, fileData);
        
        return fullPath;
    }

    public async Task<byte[]> LoadDocumentAsync(Guid paymentId, string fileName)
    {
        string path = Path.Combine(paymentId.ToString(), fileName);
        return await _storageProvider.LoadAsync(path);
    }

    public async Task DeleteDocumentAsync(Guid paymentId, string fileName)
    {
        string path = $"{paymentId}/{fileName}";
        await _storageProvider.DeleteAsync(path);
    }

    private string GetUniqueFileName(string originalFileName)
    {
        string fileExtension = Path.GetExtension(originalFileName);
        
        return $"{Guid.NewGuid()}{fileExtension}";
    }
}