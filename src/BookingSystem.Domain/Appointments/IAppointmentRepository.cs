namespace BookingSystem.Domain.Appointments;

public interface IAppointmentRepository
{
    Task Insert(Appointment appointment, CancellationToken cancellationToken);

    Task Remove(Appointment appointment, CancellationToken cancellationToken);
    Task<Appointment?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken);
}