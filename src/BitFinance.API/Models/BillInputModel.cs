using System.ComponentModel.DataAnnotations;

namespace BitFinance.API.Models;

public class BillInputModel
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Category { get; set; } = null!;
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime? PaidDate { get; set; }
    
    [Required]
    public decimal AmountDue { get; set; }
    
    public decimal? AmountPaid { get; set; }
    
    public bool? IsPaid { get; set; }
    
    public bool? IsDeleted { get; set; }
}