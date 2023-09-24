using System.ComponentModel.DataAnnotations;

namespace BitFinance.API.Models;

public class BillDTO
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Category { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? PaidDate { get; set; }
    
    [Required]
    public decimal AmountDue { get; set; }
    
    public decimal? AmountPaid { get; set; }
}