using BitFinance.Business.Enums;

namespace BitFinance.API.Models.Request;

public record CreateExpenseRequest(
    string Description, 
    string Category, 
    string Status, 
    decimal Amount,
    Guid organizationId);