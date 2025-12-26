namespace BitFinance.Application.DTOs;

public record UpdateBillRequest(string Description, 
    string Category, 
    string Status, 
    DateTime DueDate, 
    DateTime? PaymentDate, 
    decimal AmountDue, 
    decimal? AmountPaid);