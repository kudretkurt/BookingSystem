using BookingSystem.Domain.Patients;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public sealed class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PatientRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertAsync(Patient patient, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(patient, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Patient?> GetPatientByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Patients
            .Include(t => t.ConnectedPsychologists).ThenInclude(t => t.Psychologist)
            .Include(t => t.Appointments)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken)
    {
        _dbContext.Update(patient);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return patient;
    }
}