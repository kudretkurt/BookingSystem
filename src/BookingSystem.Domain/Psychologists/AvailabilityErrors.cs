using BookingSystem.Shared;

namespace BookingSystem.Domain.Psychologists;

public static class AvailabilityErrors
{
    public static readonly Error RangeCanNotBeSmallerThan30AndBiggerThan60 =
        new("Availability.RangeCanNotBeSmallerThan30AndBiggerThan60",
            "Availability time range can not be smaller than 30 minutes and bigger than 60 minutes");

    public static readonly Error EndTimeCanNotBeSmallOrEqualStartTime =
        new("Availability.EndTimeCanNotBeSmallOrEqualStartTime", "End Time should not be smaller or equal Start Time");

    public static Error InvalidDate(DateOnly date)
    {
        return new Error("Availability.InvalidDate", $"Date can not be:{date}");
    }

    public static Error InvalidTime(TimeOnly time)
    {
        return new Error("Availability.DefaultStartTime", $"Time can not be :{time}");
    }
}