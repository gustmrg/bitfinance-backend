using BitFinance.Business.Enums;

namespace BitFinance.Business.Entities;

/// <summary>
/// Represents a file attached to a bill, such as a receipt or invoice scan.
/// </summary>
public class BillDocument
{
    /// <summary>
    /// Unique identifier for the document.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The ID of the bill this document is attached to.
    /// </summary>
    public Guid BillId { get; set; }

    /// <summary>
    /// Navigation property to the parent bill.
    /// </summary>
    public Bill Bill { get; set; } = null!;

    /// <summary>
    /// The generated file name used for storage.
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// The original file name as uploaded by the user.
    /// </summary>
    public string OriginalFileName { get; set; } = null!;

    /// <summary>
    /// The MIME content type of the file (e.g., "application/pdf", "image/png").
    /// </summary>
    public string ContentType { get; set; } = null!;

    /// <summary>
    /// The size of the file in bytes.
    /// </summary>
    public long FileSizeInBytes { get; set; }

    /// <summary>
    /// The path where the file is stored in the configured storage provider.
    /// </summary>
    public string StoragePath { get; set; } = null!;

    /// <summary>
    /// The type of document (e.g., invoice, receipt).
    /// </summary>
    public DocumentType DocumentType { get; set; }

    /// <summary>
    /// The storage provider used to persist this file (e.g., Local, S3).
    /// </summary>
    public StorageProvider StorageProvider { get; set; }

    /// <summary>
    /// An optional description or note about the document.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// SHA-256 hash of the file contents for integrity verification.
    /// </summary>
    public string? FileHash { get; set; }

    /// <summary>
    /// The date and time when the document was uploaded.
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    /// The ID of the user who uploaded the document, if available.
    /// </summary>
    public Guid? UploadedByUserId { get; set; }
}
