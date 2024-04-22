using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Appointments;
using BookingSystem.Domain.Patients;
using BookingSystem.Shared;
using Mapster;

namespace BookingSystem.Application.Patient.CreateAppointment;

public class CreateAppointmentCommandHandler(
    IAppointmentDomainService appointmentDomainService,
    IPatientRepository patientRepository)
    : ICommandHandler<CreateAppointmentCommand, PatientResponse>
{
    public async Task<Result<PatientResponse>> Handle(CreateAppointmentCommand command,
        CancellationToken cancellationToken)
    {
        var appointmentResult = await appointmentDomainService.CreateAppointment(command.PatientId,
            command.PsychologistId, command.Date, command.StartTime, command.EndTime, cancellationToken);

        if (appointmentResult.IsFailure) return Result.Failure<PatientResponse>(appointmentResult.Error);

        var patient = await patientRepository.GetPatientByIdAsync(command.PatientId, cancellationToken);
        return patient.Adapt<PatientResponse>();
    }
}