namespace BitFinance.Application.Common;

public class Error
{
    public ErrorType Type { get; }
    public string[] Messages { get; }
    public string? Code { get; }
    public Dictionary<string, object>? Metadata { get; }
    
    public Error(ErrorType type, params string[] messages)
    {
        Type = type;
        Messages = messages ?? [];
    }

    public Error(ErrorType type, string code, params string[] messages)
    {
        Type = type;
        Code = code;
        Messages = messages ?? [];
    }

    public Error(ErrorType type, string[] messages, Dictionary<string, object>? metadata)
    {
        Type = type;
        Messages = messages ?? [];
        Metadata = metadata;
    }
    
    public string Message => Messages.FirstOrDefault() ?? string.Empty;
}

public enum ErrorType
{
    None,
    NotFound,           // 404
    Validation,         // 400
    Unauthorized,       // 401
    Forbidden,          // 403
    Conflict,           // 409
    BusinessRule,       // 422 or 400
    Infrastructure      // 500
}