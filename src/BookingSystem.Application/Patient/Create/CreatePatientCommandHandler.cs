using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Patients;
using BookingSystem.Shared;

namespace BookingSystem.Application.Patient.Create;

internal sealed class CreatePatientCommandHandler(IPatientRepository patientRepository)
    : ICommandHandler<CreatePatientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var patientResult = Domain.Patients.Patient.Create(command.FirstName, command.LastName, command.Age);
        if (patientResult.IsFailure) return Result.Failure<Guid>(patientResult.Error);
        await patientRepository.Insert(patientResult.Value, cancellationToken);


        //check

        return patientResult.Value.Id;
    }
}