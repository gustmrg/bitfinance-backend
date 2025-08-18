using BitFinance.Business.Entities;
using BitFinance.Business.Enums;

namespace BitFinance.API.Services.Interfaces;

public interface IBillDocumentService
{
    Task<BillDocument> UploadDocumentAsync(Guid billId, Stream fileStream, string fileName, string contentType, DocumentType documentType, Guid? userId = null);
    Task<(Stream stream, string fileName, string contentType)> GetDocumentAsync(Guid documentId);
    Task<bool> DeleteDocumentAsync(Guid documentId);
}