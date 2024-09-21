using BitFinance.Business.Enums;

namespace BitFinance.API.Models.Request;

public record CreateBillRequest(
    string Name, 
    BillCategory Category, 
    BillStatus Status, 
    DateTime DueDate, 
    DateTime? PaidDate, 
    decimal AmountDue, 
    decimal? AmountPaid);