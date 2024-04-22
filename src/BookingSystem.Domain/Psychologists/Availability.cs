using BookingSystem.Domain.Common;
using BookingSystem.Shared;

namespace BookingSystem.Domain.Psychologists;

public sealed class Availability : ValueObject
{
    private Availability(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public DateOnly Date { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }

    public static Result<List<Availability>> Create(DateOnly from, DateOnly to, TimeOnly startTime, TimeOnly endTime)
    {
        if (from == default || to == default)
            return Result.Failure<List<Availability>>(AvailabilityErrors.InvalidDate(from));

        if (startTime == default) return Result.Failure<List<Availability>>(AvailabilityErrors.InvalidTime(startTime));

        if (endTime == default) return Result.Failure<List<Availability>>(AvailabilityErrors.InvalidTime(endTime));

        if (endTime <= startTime)
            return Result.Failure<List<Availability>>(AvailabilityErrors.EndTimeCanNotBeSmallOrEqualStartTime);

        if ((endTime - startTime).TotalMinutes < 30 || (endTime - startTime).TotalMinutes > 60)
            return Result.Failure<List<Availability>>(AvailabilityErrors.RangeCanNotBeSmallerThan30AndBiggerThan60);

        if (from == to) return Result.Success(new List<Availability> { new(from, startTime, endTime) });

        var list = new List<Availability>();

        for (var i = 0; i <= to.DayNumber - from.DayNumber; i++)
            list.Add(new Availability(from.AddDays(i), startTime, endTime));

        return Result.Success(list);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Date;
        yield return StartTime;
        yield return EndTime;
    }
}