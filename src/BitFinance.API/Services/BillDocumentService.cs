using BitFinance.API.Services.Interfaces;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Contexts;
using BitFinance.Data.Repositories.Interfaces;

namespace BitFinance.API.Services;

public class BillDocumentService : IBillDocumentService
{
    private readonly IFileStorageService _storageService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BillDocumentService> _logger;
    private readonly IBillsRepository _billsRepository;
    
    private const string DocumentTypeFolder = "bills";

    public BillDocumentService(
        IFileStorageService storageService, 
        ApplicationDbContext context, 
        ILogger<BillDocumentService> logger, IBillsRepository billsRepository)
    {
        _storageService = storageService;
        _context = context;
        _logger = logger;
        _billsRepository = billsRepository;
    }
    
    public async Task<BillDocument> UploadDocumentAsync(
        Guid billId, 
        Stream fileStream, 
        string fileName, 
        string contentType, 
        DocumentType documentType,
        Guid? userId = null)
    {
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
        
        _context.BillDocuments.Add(document);
        await _context.SaveChangesAsync();

        return document;
    }

    public async Task<(Stream stream, string fileName, string contentType)> GetDocumentAsync(Guid documentId)
    {
        var document = await _context.BillDocuments.FindAsync(documentId);
        if (document == null || document.DeletedAt != null)
            throw new KeyNotFoundException($"Document {documentId} not found");

        var stream = await _storageService.GetFileAsync(document.StoragePath);
        return (stream, document.OriginalFileName, document.ContentType);
    }

    public async Task<bool> DeleteDocumentAsync(Guid documentId)
    {
        var document = await _context.BillDocuments.FindAsync(documentId);
        if (document == null)
            return false;
        
        document.DeletedAt = DateTime.UtcNow;
        
        // Optionally delete the physical file
        // await _storageService.DeleteFileAsync(document.StoragePath);
        
        await _context.SaveChangesAsync();
        return true;
    }
}