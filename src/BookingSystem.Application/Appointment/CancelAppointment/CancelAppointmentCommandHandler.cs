using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Appointments;
using BookingSystem.Shared;

namespace BookingSystem.Application.Appointment.CancelAppointment;

public class CancelAppointmentCommandHandler(IAppointmentDomainService appointmentDomainService)
    : ICommandHandler<CancelAppointmentCommand, string>
{
    public async Task<Result<string>> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken)
    {
        var cancelAppointmentResult = await
            appointmentDomainService.CancelAppointment(command.AppointmentId, cancellationToken);
        if (cancelAppointmentResult.IsFailure) return Result.Failure<string>(cancelAppointmentResult.Error);

        return Result.Success<string>("Appointment successfully cancelled");
    }
}