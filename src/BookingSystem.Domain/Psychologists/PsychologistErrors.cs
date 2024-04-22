using BookingSystem.Shared;

namespace BookingSystem.Domain.Psychologists;

public static class PsychologistErrors
{
    public static readonly Error EmptyFirstName = new("Psychologist.EmptyFirstName", "Empty First Name");
    public static readonly Error EmptyLastName = new("Psychologist.EmptyLastName", "Empty Last Name");


    public static readonly Error CanNotDeletePassedAppointment = new("Psychologist.CanNotDeletePassedAppointment",
        "You can not delete appointment which is already passed");

    public static readonly Error Availability1HourRule = new("Psychologist.Availability1HourRule",
        "You can not change existing appointment if you have less than 1 hour to meet with patient");

    public static readonly Error AppointmentDoesNotExist =
        new("Psychologist.AppointmentDoesNotExist", "You dont have this appointment");

    public static Error AvailabilityNotFound(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return new Error("Psychologist.AvailabilityNotFound",
            $"The availability which is in {date} between {startTime} and {endTime} does not exist");
    }

    public static Error AvailabilityNotUnique(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return new Error("Psychologist.AvailabilityNotUnique",
            $"You can not add existing availability : Date:{date} StartTime:{startTime} EndTime:{endTime}");
    }

    public static Error PsychologistNotExists(Guid psychologistId)
    {
        return new Error("Psychologist.PsychologistNotExists", $"Psychologist Id:{psychologistId} does not exist");
    }
}