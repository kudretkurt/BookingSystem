using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Common;
using BookingSystem.Shared;

namespace BookingSystem.Domain.Psychologists;

public class Psychologist : Entity
{
    private Psychologist(Guid id, string firstName, string lastName) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Availabilities = new List<Availability>();
        Appointments = new List<Appointment>();
    }

    private Psychologist()
    {
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public List<Availability> Availabilities { get; }
    public List<Appointment> Appointments { get; }

    public static Result<Psychologist> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<Psychologist>(PsychologistErrors.EmptyFirstName);

        if (string.IsNullOrWhiteSpace(lastName)) return Result.Failure<Psychologist>(PsychologistErrors.EmptyLastName);

        return new Psychologist(Guid.NewGuid(), firstName, lastName);
    }

    public Result CreateAvailability(DateOnly from, DateOnly to, TimeOnly startTime, TimeOnly endTime)
    {
        var result = Availability.Create(from, to, startTime, endTime);

        if (result.IsFailure) return result;

        foreach (var availability in result.Value)
            if (Availabilities.Contains(availability))
                return PsychologistErrors.AvailabilityNotUnique(availability.Date, availability.StartTime,
                    availability.EndTime);

        foreach (var availability in result.Value) Availabilities.Add(availability);

        return Result.Success(result.Value);
    }

    public Result RemoveAvailability(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var result = Availability.Create(date, date, startTime, endTime);

        if (result.IsFailure) return result;

        foreach (var availability in result.Value)
            if (!Availabilities.Contains(availability))
                return PsychologistErrors.AvailabilityNotFound(date, startTime, endTime);

        foreach (var availability in result.Value) Availabilities.Remove(availability);

        return Result.Success(result.Value);
    }

    public Result EditAvailability(DateOnly date, TimeOnly startTime, TimeOnly endTime, DateOnly newDate,
        TimeOnly newStartTime, TimeOnly newEndTime)
    {
        var currentAvailabilityResult = Availability.Create(date, date, startTime, endTime);
        if (currentAvailabilityResult.IsFailure) return currentAvailabilityResult.Error;

        var changedAvailabilityResult = Availability.Create(newDate, newDate, newStartTime, newEndTime);
        if (changedAvailabilityResult.IsFailure) return changedAvailabilityResult.Error;

        var currentAvailability = currentAvailabilityResult.Value.First();
        var changedAvailability = changedAvailabilityResult.Value.First();

        if (!Availabilities.Contains(currentAvailability))
            return PsychologistErrors.AvailabilityNotFound(currentAvailability.Date,
                currentAvailability.StartTime, currentAvailability.EndTime);

        Availabilities.Remove(currentAvailability);
        Availabilities.Add(changedAvailability);

        return Result.Success(changedAvailability);
    }

    public Result CancelAppointment(Guid appointmentId)
    {
        var appointment = Appointments.FirstOrDefault(t => t.Id == appointmentId);
        if (appointment == null) return PsychologistErrors.AppointmentDoesNotExist;

        var startTime = appointment.StartTime;
        var appointmentExactDate = appointment.Date.ToDateTime(startTime);

        if (appointmentExactDate < DateTime.UtcNow) return PsychologistErrors.CanNotDeletePassedAppointment;

        var gap = appointmentExactDate - DateTime.UtcNow;
        if (gap.TotalMinutes <= 60) return PsychologistErrors.Availability1HourRule;

        Appointments.Remove(appointment);
        var availabilityReturnedBack = Availability.Create(appointment.Date, appointment.Date, appointment.StartTime,
            appointment.EndTime);
        
        if (availabilityReturnedBack.IsFailure) return availabilityReturnedBack.Error;
        Availabilities.Add(availabilityReturnedBack.Value.First());
        return Result.Success(this);
    }
}