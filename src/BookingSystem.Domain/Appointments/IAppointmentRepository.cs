namespace BookingSystem.Domain.Appointments;

public interface IAppointmentRepository
{
    Task InsertAsync(Appointment appointment, CancellationToken cancellationToken);

    Task RemoveAsync(Appointment appointment, CancellationToken cancellationToken);
    Task<Appointment?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken);
}