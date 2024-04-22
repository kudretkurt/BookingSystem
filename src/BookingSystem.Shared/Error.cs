namespace BookingSystem.Shared;

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public static implicit operator Result(Error error)
    {
        return Result.Failure(error);
    }

    public Result ToResult()
    {
        return Result.Failure(this);
    }
}