using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Patient.ConnectWithPsychologist;

public sealed record ConnectWithPsychologistCommand(Guid PatientId, Guid PsychologistId)
    : ICommand<string>;