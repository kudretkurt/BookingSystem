using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Patient.Create;

public sealed record CreatePatientCommand(string FirstName, string LastName, int Age)
    : ICommand<Guid>;