using FluentValidation.Results;

namespace BitFinance.API.Services.Interfaces;

/// <summary>
/// Validates uploaded files against size, type, and content rules.
/// </summary>
public interface IFileValidationService
{
    /// <summary>
    /// Validates a file based on its stream, name, size, and content type.
    /// </summary>
    /// <param name="fileStream">The file content stream.</param>
    /// <param name="fileName">The original file name.</param>
    /// <param name="fileSize">The file size in bytes.</param>
    /// <param name="contentType">The MIME content type of the file.</param>
    /// <returns>A <see cref="FileValidationResult"/> indicating whether the file is valid.</returns>
    FileValidationResult ValidateFile(Stream fileStream, string fileName, long fileSize, string contentType);
}

/// <summary>
/// Contains the result of a file validation operation.
/// </summary>
public class FileValidationResult
{
    /// <summary>
    /// Indicates whether the file passed all validation checks.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// The list of validation error messages, if any.
    /// </summary>
    public List<string> Errors { get; set; } = [];

    /// <summary>
    /// A semicolon-delimited string of all validation errors.
    /// </summary>
    public string ErrorMessage => string.Join("; ", Errors);
}
