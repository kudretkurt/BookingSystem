namespace BookingSystem.Application.Common;

public sealed record Availability
{
    public DateOnly Date { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}