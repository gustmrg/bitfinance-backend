using BitFinance.API.Models;
using BitFinance.API.Services.Interfaces;

namespace BitFinance.API.Services;

public class S3StorageService : IStorageService
{
    public bool ValidateFile(IFormFile file, out string errorMessage)
    {
        throw new NotImplementedException();
    }

    public Task<string> SaveFileAsync(FileUploadDTO fileUploadDto)
    {
        throw new NotImplementedException();
    }
}