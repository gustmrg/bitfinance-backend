using BitFinance.Domain.Enums;

namespace BitFinance.Application.DTOs;

public record DocumentSummary
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public DocumentType DocumentType { get; set; }
}
