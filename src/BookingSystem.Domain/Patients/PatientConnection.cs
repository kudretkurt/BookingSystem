using BookingSystem.Domain.Common;
using BookingSystem.Domain.Psychologists;

namespace BookingSystem.Domain.Patients;

public sealed class PatientConnection : Entity
{
    public Psychologist Psychologist { get; set; }
    public Guid PsychologistId { get; set; }
    public Guid PatientId { get; set; }
}