using BitFinance.API.Services.Interfaces;
using BitFinance.Business.Enums;

namespace BitFinance.API.Services;

public class FilesService : IFilesService
{
    private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg", ".pdf" };
    private readonly string[] permittedMimeTypes = { "image/jpeg", "image/png", "application/pdf" };
    private const long FileSizeLimit = 5 * 1024 * 1024; // 5 MB

    public bool Validate(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
        {
            return false;
        }
        
        if (!permittedMimeTypes.Contains(file.ContentType))
        {
            return false;
        }
        
        if (file.Length > FileSizeLimit)
        {
            return false;
        }

        return true;
    }

    public string Save(IFormFile file, DocumentType documentType)
    {
        var safeFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        Directory.CreateDirectory(uploadsFolder);
        var filePath = Path.Combine(uploadsFolder, safeFileName);

        return filePath;
    }
}