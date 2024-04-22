using BookingSystem.API.Extensions;
using BookingSystem.Application.Patient.CancelAppointment;
using BookingSystem.Application.Patient.ConnectWithPsychologist;
using BookingSystem.Application.Patient.Create;
using BookingSystem.Application.Patient.CreateAppointment;
using BookingSystem.Application.Patient.GetById;
using MediatR;

namespace BookingSystem.API.Endpoints;

public static class PatientEndpoints
{
    public static void MapPatientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/patients",
            async (CreatePatientCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

        app.MapPatch("api/patients/connect-with-psychologist", async (ConnectWithPsychologistCommand command,
            ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapPatch("api/patients/create-appointments", async (CreateAppointmentCommand command,
            ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapDelete("api/patients/cancel-appointment/{appointmentId}", async (Guid appointmentId,
            ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CancelAppointmentCommand(appointmentId), cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapGet("api/patients/{patientId}",
            async (Guid patientId, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetPatientByIdQuery(patientId), cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });
    }
}