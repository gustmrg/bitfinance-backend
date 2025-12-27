namespace BitFinance.Domain.Common;

public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public Dictionary<string, object>? Metadata { get; }

    private Error(string code, string message, ErrorType type, Dictionary<string, object>? metadata = null)
    {
        Code = code;
        Message = message;
        Type = type;
        Metadata = metadata;
    }

    public static Error Validation(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Validation, metadata);

    public static Error NotFound(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.NotFound, metadata);

    public static Error Conflict(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Conflict, metadata);

    public static Error Unauthorized(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Unauthorized, metadata);

    public static Error Forbidden(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Forbidden, metadata);

    public static Error Failure(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Failure, metadata);

    public static Error Unexpected(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Unexpected, metadata);

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = Validation("Error.NullValue", "A null value was provided");
}
