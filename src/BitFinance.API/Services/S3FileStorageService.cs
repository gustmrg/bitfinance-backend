using System.Net;
using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Model;
using BitFinance.API.Services.Interfaces;
using BitFinance.API.Settings;
using Microsoft.Extensions.Options;

namespace BitFinance.API.Services;

public class S3FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly StorageSettings _settings;
    private readonly ILogger<S3FileStorageService> _logger;

    public S3FileStorageService(
        IAmazonS3 s3Client,
        IOptions<StorageSettings> settings,
        ILogger<S3FileStorageService> logger)
    {
        _s3Client = s3Client;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<FileStorageResult> SaveFileAsync(Stream fileStream, string fileName, string contentType, Guid entityId, string subDirectory = "")
    {
        try
        {
            var uniqueFileName = GenerateUniqueFileName(fileName);
            var key = BuildObjectKey(subDirectory, entityId, uniqueFileName);

            using var sha256 = SHA256.Create();
            using var memoryStream = new MemoryStream();

            var buffer = new byte[8192];
            int bytesRead;
            long totalBytes = 0;

            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await memoryStream.WriteAsync(buffer, 0, bytesRead);
                sha256.TransformBlock(buffer, 0, bytesRead, null, 0);
                totalBytes += bytesRead;
            }

            sha256.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            var hash = Convert.ToBase64String(sha256.Hash!);

            memoryStream.Position = 0;

            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.S3.BucketName,
                Key = key,
                InputStream = memoryStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(putRequest);

            return new FileStorageResult
            {
                Success = true,
                StoragePath = key,
                FileName = uniqueFileName,
                FileSizeInBytes = totalBytes,
                FileHash = hash
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file {FileName} to S3", fileName);
            return new FileStorageResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<Stream> GetFileAsync(string storagePath)
    {
        var response = await _s3Client.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _settings.S3.BucketName,
            Key = storagePath
        });

        return response.ResponseStream;
    }

    public async Task<bool> DeleteFileAsync(string storagePath)
    {
        try
        {
            var response = await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _settings.S3.BucketName,
                Key = storagePath
            });

            return response.HttpStatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {StoragePath} from S3", storagePath);
            return false;
        }
    }

    public async Task<bool> FileExistsAsync(string storagePath)
    {
        try
        {
            await _s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest
            {
                BucketName = _settings.S3.BucketName,
                Key = storagePath
            });

            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var guid = Guid.NewGuid().ToString("N")[..8];

        return $"{fileNameWithoutExtension}_{timestamp}_{guid}{extension}";
    }

    private string BuildObjectKey(string subDirectory, Guid entityId, string fileName)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(_settings.S3.Prefix))
            parts.Add(_settings.S3.Prefix);

        if (!string.IsNullOrWhiteSpace(subDirectory))
            parts.Add(subDirectory);

        parts.Add(DateTime.UtcNow.Year.ToString());
        parts.Add(DateTime.UtcNow.Month.ToString("00"));
        parts.Add(entityId.ToString());
        parts.Add(fileName);

        return string.Join("/", parts);
    }
}