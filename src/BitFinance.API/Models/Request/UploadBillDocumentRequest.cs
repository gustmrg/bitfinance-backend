using System.ComponentModel.DataAnnotations;
using BitFinance.API.Attributes;
using BitFinance.Domain.Enums;

namespace BitFinance.API.Models.Request;

public class UploadBillDocumentRequest
{
    [Required]
    [AllowedFileExtensions(".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx")]
    [MaxFileSize(10 * 1024 * 1024)]
    public IFormFile File { get; set; } = null!;
    
    [Required]
    public DocumentType DocumentType { get; set; }
}