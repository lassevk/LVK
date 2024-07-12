using JetBrains.Annotations;

namespace LVK.Core.Results;

public readonly struct Result
{
    private Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    [MustUseReturnValue]
    public static Result Success() => new(true, Error.None);

    [MustUseReturnValue]
    public static Result Failure(Error error) => new(false, error);

    [MustUseReturnValue]
    public static Result<T> Success<T>(T value) => new(true, value, Error.None);

    [MustUseReturnValue]
    public static Result<T> Failure<T>(Error error) => new(false, default!, error);

    public void Match(Action onSuccess, Action<Error> onFailure)
    {
        if (IsSuccess)
            onSuccess();
        else
            onFailure(Error);
    }

    [MustUseReturnValue]
    public T Match<T>(Func<T> onSuccess, Func<Error, T> onFailure) => IsSuccess ? onSuccess() : onFailure(Error);
}

public readonly struct Result<T>
{
    internal Result(bool isSuccess, T value, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new ArgumentException("Invalid error", nameof(error));

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public void Match(Action<T> onSuccess, Action<Error> onFailure)
    {
        if (IsSuccess)
            onSuccess(Value);
        else
            onFailure(Error);
    }

    [MustUseReturnValue]
    public T2 Match<T2>(Func<T, T2> onSuccess, Func<Error, T2> onFailure) => IsSuccess ? onSuccess(Value) : onFailure(Error);
}