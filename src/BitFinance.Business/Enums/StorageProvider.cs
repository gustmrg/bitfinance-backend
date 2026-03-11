namespace BitFinance.Business.Enums;

/// <summary>
/// Identifies the storage backend used to persist uploaded files.
/// </summary>
public enum StorageProvider
{
    /// <summary>
    /// Files stored on the local filesystem.
    /// </summary>
    Local = 1,

    /// <summary>
    /// Files stored in Azure Blob Storage.
    /// </summary>
    AzureBlobStorage = 2,

    /// <summary>
    /// Files stored in Amazon S3.
    /// </summary>
    AmazonS3 = 3,
}
