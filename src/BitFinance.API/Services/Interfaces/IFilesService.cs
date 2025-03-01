using BitFinance.Business.Enums;

namespace BitFinance.API.Services.Interfaces;

public interface IFilesService
{
    bool Validate(IFormFile file);
    string Save(IFormFile file, DocumentType documentType);
}