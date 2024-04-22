using BookingSystem.Domain.Patients;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;

namespace BookingSystem.Domain.Appointments;

public class AppointmentDomainService : IAppointmentDomainService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IPsychologistRepository _psychologistRepository;

    public AppointmentDomainService(IAppointmentRepository appointmentRepository,
        IPsychologistRepository psychologistRepository, IPatientRepository patientRepository)
    {
        _appointmentRepository = appointmentRepository;
        _psychologistRepository = psychologistRepository;
        _patientRepository = patientRepository;
    }

    public async Task<Result<Guid>> CreateAppointment(Guid patientId, Guid psychologistId, DateOnly date, TimeOnly startTime,
        TimeOnly endTime, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId, cancellationToken);
        if (patient == null) return Result.Failure<Guid>(PatientErrors.PatientNotExists(patientId));

        var psychologist = await _psychologistRepository.GetPsychologistByIdAsync(psychologistId, cancellationToken);
        if (psychologist == null) return Result.Failure<Guid>(PsychologistErrors.PsychologistNotExists(psychologistId));

        var appointmentResult = patient.CreateAppointment(psychologistId, date, startTime, endTime);
        if (appointmentResult.IsFailure) return Result.Failure<Guid>(appointmentResult.Error);

        var removeAvailabilityResult = psychologist.RemoveAvailability(date, startTime, endTime);
        if (removeAvailabilityResult.IsFailure) return Result.Failure<Guid>(removeAvailabilityResult.Error);

        var appointment = appointmentResult.Value;
        await _appointmentRepository.Insert(appointment, cancellationToken);
        return Result.Success(appointment.Id);
    }

    public async Task<Result> CancelAppointment(Guid appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
        if (appointment == null) return AppointmentErrors.AppointmentDoesNotExist;

        var patient = await _patientRepository.GetPatientByIdAsync(appointment.PatientId, cancellationToken);
        if (patient == null) return PatientErrors.PatientNotExists(appointment.PatientId);

        var cancelAppointmentByPatientResult = patient.CancelAppointment(appointmentId);
        if (cancelAppointmentByPatientResult.IsFailure) return cancelAppointmentByPatientResult.Error;

        var psychologist =
            await _psychologistRepository.GetPsychologistByIdAsync(appointment.PsychologistId, cancellationToken);
        if (psychologist == null) return PsychologistErrors.PsychologistNotExists(appointment.PsychologistId);

        var cancelAppointmentByPsychologistResult = psychologist.CancelAppointment(appointmentId);
        if (cancelAppointmentByPsychologistResult.IsFailure) return cancelAppointmentByPsychologistResult.Error;

        await _appointmentRepository.Remove(appointment, cancellationToken);
        return Result.Success();
    }
}