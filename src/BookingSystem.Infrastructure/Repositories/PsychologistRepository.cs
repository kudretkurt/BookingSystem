using BookingSystem.Domain.Psychologists;
using BookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Infrastructure.Repositories;

public sealed class PsychologistRepository : IPsychologistRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PsychologistRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(Psychologist psychologist, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(psychologist, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Psychologist?> GetPsychologistByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Psychologists
            .Include(t => t.Appointments)
            .Include(t => t.Availabilities).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Psychologist> UpdateAsync(Psychologist psychologist, CancellationToken cancellationToken)
    {
        _dbContext.Update(psychologist);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return psychologist;
    }
}