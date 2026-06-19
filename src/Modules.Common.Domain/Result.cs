namespace Modules.Common.Domain;

/// <summary>
/// Represents the result of an operation, which may have succeeded or failed.
/// </summary>
public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    private Result(T? value, bool isSuccess, string? error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, null);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default, false, error);
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
}

/// <summary>
/// Static helper for creating failure results without a value.
/// </summary>
public static class Result
{
    public static Result<T> Success<T>(T value)
    {
        return Result<T>.Success(value);
    }

    public static Result<T> Failure<T>(string error)
    {
        return Result<T>.Failure(error);
    }

    public static Result Failure(string error)
    {
        return Result<object>.Failure(error);
    }

    public static Result Success()
    {
        return Result<object>.Success(null!);
    }
}
