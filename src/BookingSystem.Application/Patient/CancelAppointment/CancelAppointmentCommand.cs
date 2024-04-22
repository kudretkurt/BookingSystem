using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Patient.CancelAppointment;

public sealed record CancelAppointmentCommand(Guid AppointmentId) : ICommand<string>;