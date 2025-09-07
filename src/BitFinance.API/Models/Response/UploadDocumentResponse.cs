using BitFinance.Domain.Enums;

namespace BitFinance.API.Models.Response;

public record UploadDocumentResponse
{
    public Guid Id { get; set; }
    public Guid BillId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public DocumentType DocumentType { get; set; } 
}