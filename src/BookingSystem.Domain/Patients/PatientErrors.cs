using BookingSystem.Shared;

namespace BookingSystem.Domain.Patients;

public static class PatientErrors
{
    public static readonly Error EmptyFirstName = new("Patient.EmptyFirstName", "Empty First Name");
    public static readonly Error EmptyLastName = new("Patient.EmptyLastName", "Empty Last Name");
    public static readonly Error EmptyAge = new("Patient.EmptyAge", "Empty Age");

    public static readonly Error CreateAppointmentJustWithConnectedPsychologist = new(
        "Patient.CreateAppointmentJustWithConnectedPsychologist",
        "You can make an appointment just with your connected psychologists");

    public static readonly Error MaxConnectedPsychologist = new("Patient.MaxConnectedPsychologist",
        "You can not be connected with more than 2 psychologists");

    public static readonly Error AlreadyConnectedPsychologist = new("Patient.AlreadyConnectedPsychologist",
        "You already connected with this psychologists");

    public static readonly Error AppointmentDoesNotExist =
        new("Patient.AppointmentDoesNotExist", "You dont have this appointment");

    public static readonly Error CanNotDeletePassedAppointment = new("Patient.CanNotDeletePassedAppointment",
        "You can not delete appointment which is already passed");

    public static readonly Error Availability1HourRule = new("Patient.Availability1HourRule",
        "You can not change existing appointment if you have less than 1 hour to meet with psychologist");

    public static Error ConflictAppointment(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        return new Error("Patient.ConflictAppointment",
            $"You have appointment which is conflicting with Date:{date} and between StartTime:{startTime} and EndTime:{endTime}");
    }

    public static Error PatientNotExists(Guid patientId)
    {
        return new Error("Patient.PatientNotExists", $"Patient Id:{patientId} does not exist");
    }

    public static Error NotConvenientAppointmentRequest(string psychologistName, DateOnly date, TimeOnly startTime,
        TimeOnly endTime)
    {
        return new Error("Patient.NotConvenientAppointmentRequest",
            $"Your psychologist who name is : {psychologistName} does not have any availability for Date:{date} and between StartTime:{startTime} and EndTime:{endTime}");
    }
}