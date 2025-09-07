using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;

namespace BitFinance.Application.Interfaces;

public interface IBillDocumentService
{
    Task<BillDocument> UploadDocumentAsync(Guid billId, Stream fileStream, string fileName, string contentType, DocumentType documentType, Guid? userId = null);
    Task<(Stream stream, string fileName, string contentType)> GetDocumentAsync(Guid documentId);
    Task<bool> DeleteDocumentAsync(Guid documentId);
}