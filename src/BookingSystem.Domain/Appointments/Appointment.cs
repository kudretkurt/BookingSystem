using BookingSystem.Domain.Common;
using BookingSystem.Shared;

namespace BookingSystem.Domain.Appointments;

public class Appointment : Entity
{
    private Appointment()
    {
    }

    private Appointment(Guid id, Guid psychologistId, Guid patientId, DateOnly date, TimeOnly startTime,
        TimeOnly endTime) : base(id)
    {
        Id = id;
        PsychologistId = psychologistId;
        PatientId = patientId;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Guid PsychologistId { get; private set; }
    public Guid PatientId { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public static Result<Appointment> Create(Guid psychologistId, Guid patientId, DateOnly date, TimeOnly startTime,
        TimeOnly endTime)
    {
        if (date == default) return Result.Failure<Appointment>(AppointmentErrors.EmptyDate);

        if (startTime == default) return Result.Failure<Appointment>(AppointmentErrors.EmptyStartTime);

        if (endTime == default) return Result.Failure<Appointment>(AppointmentErrors.EmptyEndTime);

        if (endTime < startTime)
            return Result.Failure<Appointment>(AppointmentErrors.EndTimeCanNotBeSmallerThanStartTime);

        return new Appointment(Guid.NewGuid(), psychologistId, patientId, date, startTime, endTime);
    }
}