namespace BookingSystem.Domain.Psychologists;

public interface IPsychologistRepository
{
    Task Insert(Psychologist psychologist, CancellationToken cancellationToken);
    Task<Psychologist?> GetPsychologistByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Psychologist> UpdateAsync(Psychologist psychologist, CancellationToken cancellationToken);
}