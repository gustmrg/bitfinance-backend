namespace BitFinance.API.Models.Bills;

public record CreateBillRequest(
    string Description, 
    string Category, 
    string Status, 
    DateTime DueDate, 
    DateTime? PaymentDate, 
    decimal AmountDue, 
    decimal? AmountPaid);