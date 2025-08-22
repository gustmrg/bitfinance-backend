using BitFinance.API.Services.Interfaces;

namespace BitFinance.API.Services;

public class FileValidationService : IFileValidationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileValidationService> _logger;
    
    private readonly HashSet<string> _allowedExtensions;
    private readonly HashSet<string> _allowedContentTypes;
    private readonly long _maxFileSize;
    private readonly Dictionary<string, byte[]> _fileSignatures;
    
    public FileValidationService(IConfiguration configuration, ILogger<FileValidationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        _allowedExtensions = _configuration.GetSection("Storage:AllowedExtensions")
            .Get<string[]>()
            ?.Select(x => x.ToLowerInvariant())
            .ToHashSet() ?? new HashSet<string> { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
        
        _maxFileSize = _configuration.GetValue<long>("Storage:MaxFileSizeInBytes", 10 * 1024 * 1024);
        
        _allowedContentTypes = new HashSet<string>
        {
            "application/pdf",
            "image/jpeg",
            "image/jpg",
            "image/png",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };
        
        // File signatures for additional security (magic numbers)
        _fileSignatures = new Dictionary<string, byte[]>
        {
            { ".pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 } }, // %PDF
            { ".jpg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".png", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } }
        };
    }
    
    public FileValidationResult ValidateFile(Stream fileStream, string fileName, long fileSize, string contentType)
    {
        var errors = new List<string>();
        
        if (fileSize > _maxFileSize)
        {
            var maxSizeMB = _maxFileSize / 1024 / 1024;
            errors.Add($"File size ({fileSize / 1024 / 1024}MB) exceeds maximum allowed size ({maxSizeMB}MB)");
        }
        
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
        {
            errors.Add($"File extension '{extension}' is not allowed. Allowed: {string.Join(", ", _allowedExtensions)}");
        }
        
        if (!_allowedContentTypes.Contains(contentType.ToLowerInvariant()))
        {
            errors.Add($"Content type '{contentType}' is not allowed");
        }
        
        if (!string.IsNullOrEmpty(extension) && _fileSignatures.ContainsKey(extension))
        {
            if (!VerifyFileSignature(fileStream, _fileSignatures[extension]))
            {
                errors.Add($"File content does not match the expected format for {extension} files");
            }
        }
        
        if (ContainsInvalidFileNameCharacters(fileName))
        {
            errors.Add("File name contains invalid characters");
        }

        return new FileValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        };
    }
    
    private bool VerifyFileSignature(Stream stream, byte[] expectedSignature)
    {
        if (stream.Length < expectedSignature.Length)
            return false;

        var buffer = new byte[expectedSignature.Length];
        var originalPosition = stream.Position;
        
        try
        {
            stream.Position = 0;
            stream.Read(buffer, 0, expectedSignature.Length);
            
            for (int i = 0; i < expectedSignature.Length; i++)
            {
                if (buffer[i] != expectedSignature[i])
                    return false;
            }
            
            return true;
        }
        finally
        {
            stream.Position = originalPosition;
        }
    }
    
    private bool ContainsInvalidFileNameCharacters(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var additionalInvalidPatterns = new[] { "..", "~", "`" };
        
        return fileName.IndexOfAny(invalidChars) >= 0 || 
               additionalInvalidPatterns.Any(pattern => fileName.Contains(pattern));
    }
}