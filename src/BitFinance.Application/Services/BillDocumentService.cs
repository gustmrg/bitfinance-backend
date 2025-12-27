using System.ComponentModel.DataAnnotations;
using BitFinance.Application.Interfaces;
using BitFinance.Domain.Entities;
using BitFinance.Domain.Enums;
using BitFinance.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace BitFinance.Application.Services;

public class BillDocumentService : IBillDocumentService
{
    private readonly IFileStorageService _storageService;
    private readonly IFileValidationService _fileValidationService;
    private readonly IBillDocumentsRepository _billDocumentsRepository;
    private readonly ILogger<BillDocumentService> _logger;
    private readonly IBillsRepository _billsRepository;

    private const string DocumentTypeFolder = "bills";

    public BillDocumentService(
        IFileStorageService storageService,
        IFileValidationService fileValidationService,
        IBillDocumentsRepository billDocumentsRepository,
        ILogger<BillDocumentService> logger,
        IBillsRepository billsRepository)
    {
        _storageService = storageService;
        _billDocumentsRepository = billDocumentsRepository;
        _logger = logger;
        _billsRepository = billsRepository;
        _fileValidationService = fileValidationService;
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
            StorageProvider = StorageProvider.Local,
            FileHash = storageResult.FileHash,
            UploadedAt = DateTime.UtcNow,
            UploadedByUserId = userId
        };
        
        await _billDocumentsRepository.CreateAsync(document);

        return document;
    }

    public async Task<(Stream stream, string fileName, string contentType)> GetDocumentAsync(Guid documentId)
    {
        var document = await _billDocumentsRepository.GetByIdAsync(documentId);
        if (document == null)
            throw new KeyNotFoundException($"Document {documentId} not found");

        var stream = await _storageService.GetFileAsync(document.StoragePath);
        return (stream, document.OriginalFileName, document.ContentType);
    }

    public async Task<bool> DeleteDocumentAsync(Guid documentId)
    {
        var document = await _billDocumentsRepository.GetByIdIncludingDeletedAsync(documentId);
        if (document == null)
            return false;

        await _billDocumentsRepository.DeleteAsync(document);

        // Optionally delete the physical file
        // await _storageService.DeleteFileAsync(document.StoragePath);

        return true;
    }
}