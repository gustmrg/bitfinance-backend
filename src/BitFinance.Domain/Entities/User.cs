using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BitFinance.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Organization> Organizations { get; } = [];
    public UserSettings Settings { get; set; } = new();
    
    public string FullName => $"{FirstName} {LastName}";
}