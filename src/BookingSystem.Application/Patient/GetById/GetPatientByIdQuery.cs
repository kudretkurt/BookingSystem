using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Patient.GetById;

public sealed record GetPatientByIdQuery(Guid PatientId) : IQuery<PatientResponse>;