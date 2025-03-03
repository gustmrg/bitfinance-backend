namespace BitFinance.API.Models;

public record FileUploadDTO(Guid OrganizationId, Guid BillId, string FileName, Stream FileStream);