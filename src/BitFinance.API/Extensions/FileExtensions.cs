namespace BitFinance.API.Extensions;

public static class FileExtensions
{
    public static string GetUniqueFileName(string originalFileName)
    {
        return $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(originalFileName)}";
    }
}