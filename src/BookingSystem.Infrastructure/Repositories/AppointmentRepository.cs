using BookingSystem.Domain.Appointments;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public sealed class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(Appointment appointment, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(appointment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(Appointment appointment, CancellationToken cancellationToken)
    {
        _dbContext.Remove(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
        return await _dbContext.Appointments.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}