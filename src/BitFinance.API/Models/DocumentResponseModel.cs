using BitFinance.Business.Enums;

namespace BitFinance.API.Models;

public record DocumentResponseModel
{
    public Guid Id { get; set; }
    public Guid BillId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public DocumentType DocumentType { get; set; } 
}