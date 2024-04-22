using BookingSystem.Domain.Common;

namespace BookingSystem.Domain.Appointments;

public class AppointmentValueObject : ValueObject
{
    public DateOnly Date { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Date;
        yield return StartTime;
        yield return EndTime;
    }
}