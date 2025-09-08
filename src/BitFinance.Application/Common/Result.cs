namespace BitFinance.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public Error? Error { get; private set; }
    
    private Result(bool isSuccess, T? data, Error? error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }
    
    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(Error error) => new(false, default, error);
    
    public static Result<T> NotFound(string code, string message) 
        => Failure(Error.NotFound(code, message));
    
    public static Result<T> NotFound(string message) 
        => Failure(Error.NotFound("Generic.NotFound", message));
    
    public static Result<T> ValidationFailed(string code, string message) 
        => Failure(Error.Validation(code, message));
    
    public static Result<T> ValidationFailed(string message) 
        => Failure(Error.Validation("Generic.Validation", message));
    
    public static Result<T> Unauthorized(string code, string message) 
        => Failure(Error.Unauthorized(code, message));
    
    public static Result<T> Unauthorized(string message = "Unauthorized access") 
        => Failure(Error.Unauthorized("Generic.Unauthorized", message));
    
    public static Result<T> Forbidden(string code, string message) 
        => Failure(Error.Forbidden(code, message));
    
    public static Result<T> Forbidden(string message = "Access forbidden") 
        => Failure(Error.Forbidden("Generic.Forbidden", message));
    
    public static Result<T> Conflict(string code, string message) 
        => Failure(Error.Conflict(code, message));
    
    public static Result<T> Conflict(string message) 
        => Failure(Error.Conflict("Generic.Conflict", message));
    
    public static Result<T> BusinessError(string code, string message) 
        => Failure(Error.BusinessRule(code, message));
    
    public static Result<T> BusinessError(string message) 
        => Failure(Error.BusinessRule("Generic.BusinessRule", message));
}