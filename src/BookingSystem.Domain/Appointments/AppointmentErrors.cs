using BookingSystem.Shared;

namespace BookingSystem.Domain.Appointments;

public static class AppointmentErrors
{
    public static readonly Error AppointmentDoesNotExist =
        Error.NotFound("Appointment.AppointmentDoesNotExist", "Appointment Does Not Exist");

    public static readonly Error EmptyDate = Error.Validation("Appointment.EmptyDate", "Empty Date");
    public static readonly Error EmptyStartTime = Error.Validation("Appointment.EmptyStartTime", "Empty Start Time");
    public static readonly Error EmptyEndTime = Error.Validation("Appointment.EmptyEndTime", "Empty End Time");

    public static readonly Error EndTimeCanNotBeSmallerThanStartTime =
        Error.Validation("Appointment.StartTimeCanNotBeSmallerThanEndTime",
            "End Time can not be smaller than Start Time");
}