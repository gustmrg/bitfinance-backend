namespace BitFinance.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyList<Error> Errors { get; }
    public Error FirstError => Errors.FirstOrDefault() ?? Error.None;

    protected Result(bool isSuccess, IEnumerable<Error> errors)
    {
        if (isSuccess && errors.Any())
            throw new InvalidOperationException("Success result cannot have errors");

        if (!isSuccess && !errors.Any())
            throw new InvalidOperationException("Failure result must have at least one error");

        IsSuccess = isSuccess;
        Errors = errors.ToList().AsReadOnly();
    }

    protected Result(bool isSuccess) : this(isSuccess, [])
    {
    }

    public static Result Success() => new(true);
    public static Result Failure(Error error) => new(false, [error]);
    public static Result Failure(IEnumerable<Error> errors) => new(false, errors);

    public static implicit operator Result(Error error) => Failure(error);
}
