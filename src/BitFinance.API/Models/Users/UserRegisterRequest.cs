namespace BitFinance.API.Models.Users;

public record UserRegisterRequest(string FirstName, string LastName, string Email, string Password, string ConfirmPassword);