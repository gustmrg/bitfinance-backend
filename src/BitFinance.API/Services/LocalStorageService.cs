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
    
    public bool ValidateFile(IFormFile file, out string errorMessage)
    {
        errorMessage = string.Empty;
        
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
        {
            errorMessage = "File extension not permitted.";
            return false;
        }
        
        if (!_permittedMimeTypes.Contains(file.ContentType))
        {
            errorMessage = "MIME type not permitted.";
            return false;
        }
        
        if (file.Length > FileSizeLimit)
        {
            errorMessage = "File size exceeds the limit.";
            return false;
        }
        
        if (file.FileName.Contains("..") || file.FileName.Contains("/") || file.FileName.Contains("\\"))
        {
            errorMessage = "Invalid file name.";
            return false;
        }
        
        if (!ValidateFileContent(file))
        {
            errorMessage = "File content does not match the file type.";
            return false;
        }

        return true;
    }
    
    private bool ValidateFileContent(IFormFile file)
    {
        using (var stream = file.OpenReadStream())
        {
            var buffer = new byte[8]; // Lê os primeiros 8 bytes do arquivo
            stream.Read(buffer, 0, buffer.Length);

            // Verifica a assinatura do arquivo com base nos primeiros bytes
            if (IsPdf(buffer))
            {
                return true;
            }
            else if (IsJpeg(buffer))
            {
                return true;
            }
            else if (IsPng(buffer))
            {
                return true;
            }
            // Adicione mais verificações para outros tipos de arquivo conforme necessário

            return false; // Assinatura não corresponde a nenhum tipo de arquivo permitido
        }
    }

    private bool IsPdf(byte[] buffer)
    {
        // Assinatura de um arquivo PDF: %PDF
        return buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46;
    }

    private bool IsJpeg(byte[] buffer)
    {
        // Assinatura de um arquivo JPEG: FF D8 FF
        return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
    }

    private bool IsPng(byte[] buffer)
    {
        // Assinatura de um arquivo PNG: 89 50 4E 47 0D 0A 1A 0A
        return buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47 &&
               buffer[4] == 0x0D && buffer[5] == 0x0A && buffer[6] == 0x1A && buffer[7] == 0x0A;
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
            throw new InvalidOperationException("Error when trying to save file", ex);
        }

        return filePath;
    }
}