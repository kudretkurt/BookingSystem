namespace BookingSystem.Domain.Patients;

public interface IPatientRepository
{
    Task Insert(Patient patient, CancellationToken cancellationToken);
    Task<Patient?> GetPatientByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken);
}