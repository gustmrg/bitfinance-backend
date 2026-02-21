using BitFinance.API.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace BitFinance.API.UnitTests.Services;

public class FileValidationServiceTests
{
    private readonly FileValidationService _sut;

    public FileValidationServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Storage:MaxFileSizeInBytes"] = (10 * 1024 * 1024).ToString(),
            })
            .Build();

        _sut = new FileValidationService(config, NullLogger<FileValidationService>.Instance);
    }

    [Fact]
    public void ValidateFile_ValidPdf_ReturnsValid()
    {
        // PDF magic bytes: %PDF
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34 };
        using var stream = new MemoryStream(pdfBytes);

        var result = _sut.ValidateFile(stream, "document.pdf", pdfBytes.Length, "application/pdf");

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateFile_FileTooLarge_ReturnsInvalid()
    {
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        using var stream = new MemoryStream(pdfBytes);
        var oversizedLength = 11L * 1024 * 1024; // 11MB

        var result = _sut.ValidateFile(stream, "large.pdf", oversizedLength, "application/pdf");

        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Contain("exceeds maximum allowed size");
    }

    [Fact]
    public void ValidateFile_DisallowedExtension_ReturnsInvalid()
    {
        using var stream = new MemoryStream(new byte[] { 0x4D, 0x5A }); // MZ header

        var result = _sut.ValidateFile(stream, "malware.exe", 100, "application/octet-stream");

        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Contain("extension");
    }

    [Fact]
    public void ValidateFile_MismatchedMagicBytes_ReturnsInvalid()
    {
        // JPEG magic bytes but with .pdf extension
        var jpegBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10 };
        using var stream = new MemoryStream(jpegBytes);

        var result = _sut.ValidateFile(stream, "fake.pdf", jpegBytes.Length, "application/pdf");

        result.IsValid.Should().BeFalse();
        result.ErrorMessage.Should().Contain("does not match");
    }
}
