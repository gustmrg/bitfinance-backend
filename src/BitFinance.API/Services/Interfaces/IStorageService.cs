using BitFinance.API.Models;

namespace BitFinance.API.Services.Interfaces;

public interface IStorageService
{
    bool ValidateFile(IFormFile file);
    Task<string> SaveFileAsync(FileUploadDTO fileUploadDto);
}