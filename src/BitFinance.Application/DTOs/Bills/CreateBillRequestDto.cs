namespace BitFinance.Application.DTOs.Bills;

public class CreateBillRequestDto
{
    public required string Description { get; set; }
    public required string Category { get; set; }
    public required string Status { get; set; }
    public required DateTime DueDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public required decimal AmountDue { get; set; }
    public decimal? AmountPaid { get; set; }
}