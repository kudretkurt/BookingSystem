using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Psychologist.Create;

public sealed record CreatePsychologistCommand(string FirstName, string LastName)
    : ICommand<Guid>;