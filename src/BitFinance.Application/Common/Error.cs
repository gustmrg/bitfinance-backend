namespace BitFinance.Application.Common;

public record Error
{
    public ErrorType Type { get; }
    public string Code { get; }
    public string Message { get; }
    public Dictionary<string, object>? Metadata { get; private init; }
    
    private Error(ErrorType type, string code, string message)
    {
        Type = type;
        Code = code;
        Message = message;
    }
    
    public static Error NotFound(string code, string message) => new(ErrorType.NotFound, code, message);
    public static Error Validation(string code, string message) => new(ErrorType.Validation, code, message);
    public static Error Unauthorized(string code, string message) => new(ErrorType.Unauthorized, code, message);
    public static Error Forbidden(string code, string message) => new(ErrorType.Forbidden, code, message);
    public static Error Conflict(string code, string message) => new(ErrorType.Conflict, code, message);
    public static Error BusinessRule(string code, string message) => new(ErrorType.BusinessRule, code, message);
    public static Error Infrastructure(string code, string message) => new(ErrorType.Infrastructure, code, message);
    
    public Error WithMetadata(Dictionary<string, object> metadata)
    {
        return this with { Metadata = metadata };
    }
}

public enum ErrorType
{
    NotFound,           
    Validation,         
    Unauthorized,       
    Forbidden,          
    Conflict,           
    BusinessRule,       
    Infrastructure      
}