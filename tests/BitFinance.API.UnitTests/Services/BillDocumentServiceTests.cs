using BitFinance.API.Services;
using BitFinance.API.Services.Interfaces;
using BitFinance.API.Settings;
using BitFinance.Business.Entities;
using BitFinance.Business.Enums;
using BitFinance.Data.Repositories.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace BitFinance.API.UnitTests.Services;

public class BillDocumentServiceTests
{
    private readonly IFileStorageService _storageService;
    private readonly IFileValidationService _fileValidationService;
    private readonly IBillDocumentsRepository _billDocumentsRepository;
    private readonly IBillsRepository _billsRepository;
    private readonly BillDocumentService _sut;

    public BillDocumentServiceTests()
    {
        _storageService = Substitute.For<IFileStorageService>();
        _fileValidationService = Substitute.For<IFileValidationService>();
        _billDocumentsRepository = Substitute.For<IBillDocumentsRepository>();
        _billsRepository = Substitute.For<IBillsRepository>();

        var storageSettings = Options.Create(new StorageSettings { Provider = "Local" });

        _sut = new BillDocumentService(
            _storageService,
            _fileValidationService,
            _billDocumentsRepository,
            NullLogger<BillDocumentService>.Instance,
            _billsRepository,
            storageSettings);
    }

    [Fact]
    public async Task UploadDocumentAsync_ValidInputs_ReturnsDocument()
    {
        var billId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        using var stream = new MemoryStream(new byte[] { 0x25, 0x50, 0x44, 0x46 });

        _fileValidationService.ValidateFile(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<long>(), Arg.Any<string>())
            .Returns(new FileValidationResult { IsValid = true });

        _billsRepository.GetByIdAsync(billId)
            .Returns(new Bill { Id = billId, Description = "Test", OrganizationId = Guid.NewGuid() });

        _storageService.SaveFileAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<string>())
            .Returns(new FileStorageResult
            {
                Success = true,
                StoragePath = "bills/test.pdf",
                FileName = "unique.pdf",
                FileSizeInBytes = 4,
                FileHash = "abc123"
            });

        var result = await _sut.UploadDocumentAsync(
            billId, stream, "test.pdf", "application/pdf", DocumentType.Receipt, userId);

        result.Should().NotBeNull();
        result.BillId.Should().Be(billId);
        result.ContentType.Should().Be("application/pdf");
        result.DocumentType.Should().Be(DocumentType.Receipt);
        result.UploadedByUserId.Should().Be(userId);
        result.StorageProvider.Should().Be(StorageProvider.Local);

        await _billDocumentsRepository.Received(1).CreateAsync(Arg.Any<BillDocument>());
    }

    [Fact]
    public async Task UploadDocumentAsync_FileValidationFails_ThrowsValidationException()
    {
        using var stream = new MemoryStream(new byte[] { 1, 2, 3 });

        _fileValidationService.ValidateFile(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<long>(), Arg.Any<string>())
            .Returns(new FileValidationResult
            {
                IsValid = false,
                Errors = ["File extension '.exe' is not allowed"]
            });

        var act = () => _sut.UploadDocumentAsync(
            Guid.NewGuid(), stream, "malware.exe", "application/octet-stream", DocumentType.Other);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task UploadDocumentAsync_BillNotFound_ThrowsKeyNotFoundException()
    {
        var billId = Guid.NewGuid();
        using var stream = new MemoryStream(new byte[] { 1 });

        _fileValidationService.ValidateFile(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<long>(), Arg.Any<string>())
            .Returns(new FileValidationResult { IsValid = true });

        _billsRepository.GetByIdAsync(billId).Returns((Bill?)null);

        var act = () => _sut.UploadDocumentAsync(
            billId, stream, "test.pdf", "application/pdf", DocumentType.Receipt);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{billId}*");
    }

    [Fact]
    public async Task UploadDocumentAsync_StorageFails_ThrowsException()
    {
        var billId = Guid.NewGuid();
        using var stream = new MemoryStream(new byte[] { 1 });

        _fileValidationService.ValidateFile(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<long>(), Arg.Any<string>())
            .Returns(new FileValidationResult { IsValid = true });

        _billsRepository.GetByIdAsync(billId)
            .Returns(new Bill { Id = billId, Description = "Test", OrganizationId = Guid.NewGuid() });

        _storageService.SaveFileAsync(Arg.Any<Stream>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<string>())
            .Returns(new FileStorageResult { Success = false, ErrorMessage = "Disk full" });

        var act = () => _sut.UploadDocumentAsync(
            billId, stream, "test.pdf", "application/pdf", DocumentType.Receipt);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Failed to save file*");
    }

    [Fact]
    public async Task DeleteDocumentAsync_DocumentExists_DeletesAndReturnsTrue()
    {
        var documentId = Guid.NewGuid();
        var document = new BillDocument
        {
            Id = documentId,
            StoragePath = "bills/test.pdf",
            FileName = "test.pdf",
            OriginalFileName = "test.pdf",
            ContentType = "application/pdf"
        };

        _billDocumentsRepository.GetByIdAsync(documentId).Returns(document);

        var result = await _sut.DeleteDocumentAsync(documentId);

        result.Should().BeTrue();
        await _storageService.Received(1).DeleteFileAsync("bills/test.pdf");
        await _billDocumentsRepository.Received(1).DeleteAsync(document);
    }

    [Fact]
    public async Task DeleteDocumentAsync_DocumentNotFound_ReturnsFalse()
    {
        var documentId = Guid.NewGuid();
        _billDocumentsRepository.GetByIdAsync(documentId).Returns((BillDocument?)null);

        var result = await _sut.DeleteDocumentAsync(documentId);

        result.Should().BeFalse();
        await _storageService.DidNotReceive().DeleteFileAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task GetDocumentAsync_DocumentNotFound_ThrowsKeyNotFoundException()
    {
        var documentId = Guid.NewGuid();
        _billDocumentsRepository.GetByIdAsync(documentId).Returns((BillDocument?)null);

        var act = () => _sut.GetDocumentAsync(documentId);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{documentId}*");
    }
}
