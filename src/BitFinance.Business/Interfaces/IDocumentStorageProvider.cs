namespace BitFinance.Business.Interfaces;

public interface IDocumentStorageProvider
{
    Task SaveAsync(string path, byte[] fileData);
    Task<byte[]> LoadAsync(string path);
    Task DeleteAsync(string path);
}