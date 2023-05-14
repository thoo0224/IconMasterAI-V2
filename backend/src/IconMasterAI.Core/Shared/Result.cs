namespace IconMasterAI.Core.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }

    public bool IsFailure => !IsSuccess;

    protected internal Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) 
        => new(default, false, error);

    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true, Error.None);
}

public class Result<TValue> : Result
{
    public TValue? Value { get; set; }

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}