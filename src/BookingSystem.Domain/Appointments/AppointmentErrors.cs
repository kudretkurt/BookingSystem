using BookingSystem.Shared;

namespace BookingSystem.Domain.Appointments;

public static class AppointmentErrors
{
    public static readonly Error AppointmentDoesNotExist =
        new("Appointment.AppointmentDoesNotExist", "Appointment Does Not Exist");

    public static readonly Error EmptyDate = new("Appointment.EmptyDate", "Empty Date");
    public static readonly Error EmptyStartTime = new("Appointment.EmptyStartTime", "Empty Start Time");
    public static readonly Error EmptyEndTime = new("Appointment.EmptyEndTime", "Empty End Time");

    public static readonly Error EndTimeCanNotBeSmallerThanStartTime =
        new("Appointment.StartTimeCanNotBeSmallerThanEndTime", "End Time can not be smaller than Start Time");
}