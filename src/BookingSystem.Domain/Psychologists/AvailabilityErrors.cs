using BookingSystem.Shared;

namespace BookingSystem.Domain.Psychologists;

public static class AvailabilityErrors
{
    public static readonly Error RangeCanNotBeSmallerThan30AndBiggerThan60 =
        Error.Validation("Availability.RangeCanNotBeSmallerThan30AndBiggerThan60",
            "Availability time range can not be smaller than 30 minutes and bigger than 60 minutes");

    public static readonly Error EndTimeCanNotBeSmallOrEqualStartTime =
        Error.Validation("Availability.EndTimeCanNotBeSmallOrEqualStartTime",
            "End Time should not be smaller or equal Start Time");

    public static Error InvalidDate(DateOnly date)
    {
        return Error.Validation("Availability.InvalidDate", $"Date can not be:{date}");
    }

    public static Error InvalidTime(TimeOnly time)
    {
        return Error.Validation("Availability.DefaultStartTime", $"Time can not be :{time}");
    }
}