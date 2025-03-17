using BitFinance.API.Models;

namespace BitFinance.API.Services.Interfaces;

public interface IStorageService
{
    bool ValidateFile(IFormFile file, out string errorMessage);
    Task<string> SaveFileAsync(FileUploadDTO fileUploadDto);
}