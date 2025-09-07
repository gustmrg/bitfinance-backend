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
    
    public static Result<T> NotFound(string message) 
        => Failure(new Error(ErrorType.NotFound, message));
    
    public static Result<T> ValidationFailed(params string[] errors) 
        => Failure(new Error(ErrorType.Validation, errors));
    
    public static Result<T> Unauthorized(string message = "Unauthorized access") 
        => Failure(new Error(ErrorType.Unauthorized, message));
    
    public static Result<T> Forbidden(string message = "Access forbidden") 
        => Failure(new Error(ErrorType.Forbidden, message));
    
    public static Result<T> Conflict(string message) 
        => Failure(new Error(ErrorType.Conflict, message));
    
    public static Result<T> BusinessError(string message) 
        => Failure(new Error(ErrorType.BusinessRule, message));
}