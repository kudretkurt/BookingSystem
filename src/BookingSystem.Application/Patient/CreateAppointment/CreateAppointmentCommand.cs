using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Patient.CreateAppointment;

public sealed record CreateAppointmentCommand(
    Guid PatientId,
    Guid PsychologistId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime)
    : ICommand<PatientResponse>;