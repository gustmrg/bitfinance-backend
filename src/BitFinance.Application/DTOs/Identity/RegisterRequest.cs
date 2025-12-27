namespace BitFinance.Application.DTOs.Identity;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password, string ConfirmPassword);
