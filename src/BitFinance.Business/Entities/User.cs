using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BitFinance.Business.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Organization> Organizations { get; } = [];
    
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}