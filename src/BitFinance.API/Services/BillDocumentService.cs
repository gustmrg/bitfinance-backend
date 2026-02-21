using BitFinance.API.Services.Interfaces;
using BitFinance.API.Settings;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace BitFinance.API.Services;

public class BillDocumentService : IBillDocumentService
{
    private readonly IFileStorageService _storageService;
    private readonly IFileValidationService _fileValidationService;
    private readonly IBillDocumentsRepository _billDocumentsRepository;
    private readonly ILogger<BillDocumentService> _logger;
    private readonly IBillsRepository _billsRepository;
    private readonly StorageSettings _storageSettings;

    private const string DocumentTypeFolder = "bills";

    public BillDocumentService(
        IFileStorageService storageService,
        IFileValidationService fileValidationService,
        IBillDocumentsRepository billDocumentsRepository,
        ILogger<BillDocumentService> logger,
        IBillsRepository billsRepository,
        IOptions<StorageSettings> storageSettings)
    {
        _storageService = storageService;
        _billDocumentsRepository = billDocumentsRepository;
        _logger = logger;
        _billsRepository = billsRepository;
        _fileValidationService = fileValidationService;
        _storageSettings = storageSettings.Value;
    }
    
    public async Task<BillDocument> UploadDocumentAsync(
        Guid billId, 
        Stream fileStream, 
        string fileName, 
        string contentType, 
        DocumentType documentType,
        Guid? userId = null)
    {
        var validationResult = _fileValidationService.ValidateFile(fileStream, fileName, fileStream.Length, contentType);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("File validation failed for {FileName}: {Errors}", 
                fileName, validationResult.ErrorMessage);
            throw new ValidationException(validationResult.ErrorMessage);
        }
        
        var bill = await _billsRepository.GetByIdAsync(billId);
        if (bill == null)
        {
            _logger.LogError("Bill with ID {BillId} not found.", billId);
            throw new KeyNotFoundException($"Bill with ID {billId} not found.");
        }
        
        var storageResult = await _storageService.SaveFileAsync(
            fileStream, 
            fileName, 
            contentType, 
            billId,
            DocumentTypeFolder
        );

        if (!storageResult.Success)
        {
            throw new Exception($"Failed to save file: {storageResult.ErrorMessage}");
        }
        
        var document = new BillDocument
        {
            Id = Guid.NewGuid(),
            BillId = billId,
            FileName = storageResult.FileName!,
            OriginalFileName = fileName,
            ContentType = contentType,
            FileSizeInBytes = storageResult.FileSizeInBytes ?? 0,
            StoragePath = storageResult.StoragePath!,
            DocumentType = documentType,
            StorageProvider = GetStorageProvider(),
            FileHash = storageResult.FileHash,
            UploadedAt = DateTime.UtcNow,
            UploadedByUserId = userId
        };
        
        await _billDocumentsRepository.CreateAsync(document);

        return document;
    }

    public async Task<(Stream stream, string fileName, string contentType)> GetDocumentAsync(Guid documentId)
    {
        var document = await _billDocumentsRepository.GetByIdAsync(documentId)
            ?? throw new KeyNotFoundException($"Document {documentId} not found");

        var stream = await _storageService.GetFileAsync(document.StoragePath);
        return (stream, document.OriginalFileName, document.ContentType);
    }

    public async Task<bool> DeleteDocumentAsync(Guid documentId)
    {
        var document = await _billDocumentsRepository.GetByIdAsync(documentId);
        if (document == null)
            return false;

        await _storageService.DeleteFileAsync(document.StoragePath);
        await _billDocumentsRepository.DeleteAsync(document);
        return true;
    }

    private StorageProvider GetStorageProvider() => _storageSettings.Provider switch
    {
        "S3" => StorageProvider.AmazonS3,
        _ => StorageProvider.Local
    };
}