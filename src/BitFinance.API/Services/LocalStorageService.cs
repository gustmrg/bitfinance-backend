using BitFinance.API.Models;
using BitFinance.API.Services.Interfaces;
using static BitFinance.API.Extensions.FileExtensions;

namespace BitFinance.API.Services;

public class LocalStorageService : IStorageService
{
    private const long FileSizeLimit = 5 * 1024 * 1024; // 5 MB
    private readonly string[] _permittedExtensions = [".png", ".jpg", ".jpeg", ".pdf"];
    private readonly string[] _permittedMimeTypes = ["image/jpeg", "image/png", "application/pdf"];
    private readonly string _basePath;

    public LocalStorageService()
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public bool ValidateFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
        {
            return false;
        }
        
        if (!_permittedMimeTypes.Contains(file.ContentType))
        {
            return false;
        }
        
        if (file.Length > FileSizeLimit)
        {
            return false;
        }

        return true;
    }

    public async Task<string> SaveFileAsync(FileUploadDTO fileUploadDto)
    {
        var uniqueFileName = GetUniqueFileName(fileUploadDto.FileName);
        
        var fileDirectory = Path.Combine(_basePath, fileUploadDto.OrganizationId.ToString(), fileUploadDto.BillId.ToString());
        var filePath = Path.Combine(fileDirectory, uniqueFileName);

        try
        {
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            using (var outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileUploadDto.FileStream.CopyToAsync(outputFileStream);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao salvar o arquivo", ex);
        }

        return filePath;
    }
}