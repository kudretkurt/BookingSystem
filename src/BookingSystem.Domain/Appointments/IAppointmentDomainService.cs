using BookingSystem.Shared;

namespace BookingSystem.Domain.Appointments;

public interface IAppointmentDomainService
{
    public Task<Result<Guid>> CreateAppointment(Guid patientId, Guid psychologistId, DateOnly date, TimeOnly startTime,
        TimeOnly endTime, CancellationToken cancellationToken);

    public Task<Result> CancelAppointment(Guid appointmentId, CancellationToken cancellationToken);
}