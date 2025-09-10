namespace BitFinance.Application.Common;

public static class DomainErrors
{
    public static class Bills
    {
        public static readonly Error DescriptionIsRequired = Error.Validation(
            "Bills.Description.Required",
            "A description is required");

        public static readonly Error AmountMustBePositive = Error.Validation(
            "Bills.Amount.Positive",
            "Amount due must be greater than zero.");
        
        public static readonly Error InvalidCategory = Error.Validation(
            "Bills.Category.Invalid",
            "A valid category is required.");
        
        public static readonly Error InvalidStatus = Error.Validation(
            "Bills.Status.Invalid",
            "A valid status is required.");
        
        public static readonly Error PaymentAmountMustBePositive = Error.Validation(
            "Bills.PaymentAmount.Positive",
            "Payment amount must be greater than zero when payment date is provided.");
            
        public static readonly Error NotFound = Error.NotFound(
            "Bills.NotFound",
            "No bill found.");
        
        public static readonly Error InvalidDateRange = Error.Validation(
            "Bills.DateRange.Invalid",
            "Both from and to dates must be provided, and from date must be before to date.");
    }
    
    public static class Users
    {
        public static readonly Error DuplicateEmail = Error.Conflict(
            "Users.Email.Duplicate",
            "The email provided is already in use.");
        
        public static readonly Error NotFound = Error.NotFound(
            "Users.NotFound",
            "No account found for this user.");
    }
    
    public static class Authentication
    {
        public static readonly Error InvalidCredentials = Error.Unauthorized(
            "Authentication.InvalidCredentials",
            "Invalid email or password.");
        
        public static readonly Error AccountLocked = Error.Forbidden(
            "Authentication.AccountLocked", 
            "Account is locked due to multiple failed login attempts. Try again later.");
        
        public static readonly Error AccountNotConfirmed = Error.Forbidden(
            "Authentication.NotConfirmed",
            "Email address has not been confirmed. Please check your email for confirmation instructions.");
        
        public static readonly Error UserNotFound = Error.NotFound(
            "Authentication.UserNotFound",
            "No account found with this email address.");
        
        public static readonly Error LoginFailed = Error.Unauthorized(
            "Authentication.LoginFailed",
            "Login attempt failed. Please try again.");
    }
}