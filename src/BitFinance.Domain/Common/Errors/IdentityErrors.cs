namespace BitFinance.Domain.Common.Errors;

public static class IdentityErrors
{
    public static Error InvalidCredentials => Error.Unauthorized(
        "Identity.InvalidCredentials",
        "Invalid email or password");

    public static Error UserNotFound => Error.NotFound(
        "Identity.UserNotFound",
        "User was not found");

    public static Error EmailAlreadyExists => Error.Conflict(
        "Identity.EmailAlreadyExists",
        "A user with this email already exists");

    public static Error AccountLockedOut => Error.Forbidden(
        "Identity.AccountLockedOut",
        "Account is locked out");

    public static Error PasswordTooWeak(IEnumerable<string> requirements) => Error.Validation(
        "Identity.PasswordTooWeak",
        "Password does not meet requirements",
        new Dictionary<string, object> { ["requirements"] = requirements.ToList() });

    public static Error RegistrationFailed(IEnumerable<string> errors) => Error.Validation(
        "Identity.RegistrationFailed",
        "Registration failed",
        new Dictionary<string, object> { ["errors"] = errors.ToList() });

    public static Error UpdateFailed => Error.Failure(
        "Identity.UpdateFailed",
        "Failed to update user profile");

    public static Error InvalidRefreshToken => Error.Unauthorized(
        "Identity.InvalidRefreshToken",
        "Invalid or expired refresh token");
}
