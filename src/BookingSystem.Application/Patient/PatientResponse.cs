using BookingSystem.Application.Common;

namespace BookingSystem.Application.Patient;

public sealed record PatientResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int Age { get; init; }
    public required List<UpComingAppointment> UpComingAppointments { get; init; }
    public required List<AvailableSlot> AvailableSlots { get; init; }
}

public sealed record AvailableSlot
{
    public required string PsychologistName { get; init; }
    public required string PsychologistLastName { get; init; }
    public required List<Availability> Availabilities { get; init; }
}