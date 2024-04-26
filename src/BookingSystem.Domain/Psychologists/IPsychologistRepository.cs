namespace BookingSystem.Domain.Psychologists;

public interface IPsychologistRepository
{
    Task InsertAsync(Psychologist psychologist, CancellationToken cancellationToken);
    Task<Psychologist?> GetPsychologistByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Psychologist> UpdateAsync(Psychologist psychologist, CancellationToken cancellationToken);
}