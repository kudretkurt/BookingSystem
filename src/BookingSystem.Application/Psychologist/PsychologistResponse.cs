using BookingSystem.Application.Common;

namespace BookingSystem.Application.Psychologist;

public sealed record PsychologistResponse
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required List<UpComingAppointment> UpComingAppointments { get; init; }
    public required List<Availability> Availabilities { get; init; }
}