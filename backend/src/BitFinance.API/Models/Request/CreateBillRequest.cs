using BitFinance.Business.Enums;

namespace BitFinance.API.Models;

public record CreateBillRequest(string Name, BillCategory Category, DateTime DueDate, DateTime? PaidDate, decimal AmountDue, decimal? AmountPaid);