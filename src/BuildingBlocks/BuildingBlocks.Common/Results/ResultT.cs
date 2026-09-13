namespace BuildingBlocks.Common.Results;

public class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value) : base()
    {
        _value = value;
    }

    private Result(Error error) : base(error)
    {
        _value = default;
    }

    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }
            return _value!;
        }
    }
    
    public static implicit operator Result<T>(Error error) =>
        new(error);

    public static implicit operator Result<T>(T value) =>
        new(value);
    
    public static Result<T> Success(T value) => new Result<T>(value);
    
    public static Result<T> Failure(Error error) => new Result<T>(error);
    
}