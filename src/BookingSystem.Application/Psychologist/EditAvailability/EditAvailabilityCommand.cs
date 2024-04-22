using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Psychologist.EditAvailability;

public sealed record EditAvailabilityCommand(
    Guid PsychologistId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    DateOnly NewDate,
    TimeOnly NewStartTime,
    TimeOnly NewEndTime)
    : ICommand<PsychologistResponse>;