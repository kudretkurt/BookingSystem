using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Psychologist.CreateAvailability;

public sealed record CreateAvailabilityCommand(
    Guid PsychologistId,
    DateOnly From,
    DateOnly To,
    TimeOnly StartTime,
    TimeOnly EndTime)
    : ICommand<PsychologistResponse>;