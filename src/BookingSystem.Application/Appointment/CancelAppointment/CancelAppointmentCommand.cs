using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Appointment.CancelAppointment;

public sealed record CancelAppointmentCommand(Guid AppointmentId) : ICommand<string>;