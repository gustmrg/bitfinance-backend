namespace BitFinance.Application.DTOs.Identity;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
