using BitFinance.Business.Entities;
using BitFinance.Business.Enums;

namespace BitFinance.API.Services.Interfaces;

/// <summary>
/// Manages file attachments (documents) associated with bills.
/// </summary>
public interface IBillDocumentService
{
    /// <summary>
    /// Uploads a document and attaches it to the specified bill.
    /// </summary>
    /// <param name="billId">The ID of the bill to attach the document to.</param>
    /// <param name="fileStream">The file content stream.</param>
    /// <param name="fileName">The original file name.</param>
    /// <param name="contentType">The MIME content type of the file.</param>
    /// <param name="documentType">The type of document being uploaded.</param>
    /// <param name="userId">The ID of the user uploading the document.</param>
    /// <returns>The created <see cref="BillDocument"/> entity.</returns>
    Task<BillDocument> UploadDocumentAsync(Guid billId, Stream fileStream, string fileName, string contentType, DocumentType documentType, Guid? userId = null);

    /// <summary>
    /// Retrieves the file stream and metadata for a document.
    /// </summary>
    /// <param name="documentId">The ID of the document to retrieve.</param>
    /// <returns>A tuple containing the file stream, file name, and content type.</returns>
    Task<(Stream stream, string fileName, string contentType)> GetDocumentAsync(Guid documentId);

    /// <summary>
    /// Deletes a document and its associated file from storage.
    /// </summary>
    /// <param name="documentId">The ID of the document to delete.</param>
    /// <returns><c>true</c> if the document was successfully deleted; otherwise, <c>false</c>.</returns>
    Task<bool> DeleteDocumentAsync(Guid documentId);
}
