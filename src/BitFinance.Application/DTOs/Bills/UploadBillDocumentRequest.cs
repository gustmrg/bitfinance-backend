using System.ComponentModel.DataAnnotations;
using BitFinance.Application.Attributes;
using BitFinance.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BitFinance.Application.DTOs.Bills;

public class UploadBillDocumentRequest
{
    [Required]
    [AllowedFileExtensions(".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx")]
    [MaxFileSize(10 * 1024 * 1024)]
    public IFormFile File { get; set; } = null!;

    [Required]
    public DocumentType DocumentType { get; set; }
}
