using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Patients;
using BookingSystem.Shared;
using Mapster;

namespace BookingSystem.Application.Patient.GetById;

public class GetPatientByIdQueryHandler(IPatientRepository patientRepository)
    : IQueryHandler<GetPatientByIdQuery, PatientResponse>
{
    public async Task<Result<PatientResponse>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetPatientByIdAsync(request.PatientId, cancellationToken);
        if (patient == null) return Result.Failure<PatientResponse>(PatientErrors.PatientNotExists(request.PatientId));

        var patientResponse = patient.Adapt<PatientResponse>();
        return patientResponse;
    }
}