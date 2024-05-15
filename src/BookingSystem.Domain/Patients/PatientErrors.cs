using BookingSystem.Shared;

namespace BookingSystem.Domain.Patients;

public static class PatientErrors
{
    public static readonly Error EmptyFirstName = Error.Validation("Patient.EmptyFirstName", "Empty First Name");
    public static readonly Error EmptyLastName = Error.Validation("Patient.EmptyLastName", "Empty Last Name");
    public static readonly Error EmptyAge = Error.Validation("Patient.EmptyAge", "Empty Age");

    public static readonly Error CreateAppointmentJustWithConnectedPsychologist = Error.Validation(
        "Patient.CreateAppointmentJustWithConnectedPsychologist",
        "You can make an appointment just with your connected psychologists");

    public static readonly Error MaxConnectedPsychologist = Error.Validation("Patient.MaxConnectedPsychologist",
        "You can not be connected with more than 2 psychologists");

    public static readonly Error AlreadyConnectedPsychologist = Error.Conflict("Patient.AlreadyConnectedPsychologist",
        "You already connected with this psychologists");

    public static readonly Error AppointmentDoesNotExist =
        Error.NotFound("Patient.AppointmentDoesNotExist", "You dont have this appointment");

    public static readonly Error CanNotDeletePassedAppointment = Error.Validation(
        "Patient.CanNotDeletePassedAppointment",
        "You can not delete appointment which is already passed");

    public static readonly Error Availability1HourRule = Error.Validation("Patient.Availability1HourRule",
        "You can not change existing appointment if you have less than 1 hour to meet with psychologist");

    public static Error ConflictAppointment(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return Error.Conflict("Patient.ConflictAppointment",
            $"You have appointment which is conflicting with Date:{date} and between StartTime:{startTime} and EndTime:{endTime}");
    }

    public static Error PatientNotExists(Guid patientId)
    {
        return Error.NotFound("Patient.PatientNotExists", $"Patient Id:{patientId} does not exist");
    }

    public static Error NotConvenientAppointmentRequest(string psychologistName, DateOnly date, TimeOnly startTime,
        TimeOnly endTime)
    {
        return Error.Conflict("Patient.NotConvenientAppointmentRequest",
            $"Your psychologist who name is : {psychologistName} does not have any availability for Date:{date} and between StartTime:{startTime} and EndTime:{endTime}");
    }
}