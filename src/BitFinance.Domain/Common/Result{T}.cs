namespace BitFinance.Domain.Common;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failed result");

    private Result(TValue value) : base(true)
    {
        _value = value;
    }

    private Result(IEnumerable<Error> errors) : base(false, errors)
    {
        _value = default;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public new static Result<TValue> Failure(Error error) => new([error]);
    public new static Result<TValue> Failure(IEnumerable<Error> errors) => new(errors);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
    public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);

    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<IReadOnlyList<Error>, TResult> onFailure)
        => IsSuccess ? onSuccess(_value!) : onFailure(Errors);

    public async Task<TResult> MatchAsync<TResult>(
        Func<TValue, Task<TResult>> onSuccess,
        Func<IReadOnlyList<Error>, Task<TResult>> onFailure)
        => IsSuccess ? await onSuccess(_value!) : await onFailure(Errors);
}
