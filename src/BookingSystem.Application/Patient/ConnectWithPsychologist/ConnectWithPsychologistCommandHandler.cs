using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Patients;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;

namespace BookingSystem.Application.Patient.ConnectWithPsychologist;

public class ConnectWithPsychologistCommandHandler(
    IPatientRepository patientRepository,
    IPsychologistRepository psychologistRepository)
    : ICommandHandler<ConnectWithPsychologistCommand, string>
{
    public async Task<Result<string>> Handle(ConnectWithPsychologistCommand command,
        CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetPatientByIdAsync(command.PatientId, cancellationToken);
        if (patient == null) return Result.Failure<string>(PatientErrors.PatientNotExists(command.PatientId));

        var psychologist =
            await psychologistRepository.GetPsychologistByIdAsync(command.PsychologistId, cancellationToken);
        if (psychologist == null)
            return Result.Failure<string>(PsychologistErrors.PsychologistNotExists(command.PsychologistId));

        var connectionResult = patient.ConnectPsychologist(psychologist);
        if (connectionResult.IsFailure) return Result.Failure<string>(connectionResult.Error);

        await patientRepository.UpdateAsync(patient, cancellationToken);

        return Result.Success(
            $"Patient : {patient.FirstName} {patient.LastName} connected with Psychologist : {psychologist.FirstName} {psychologist.LastName}");
    }
}