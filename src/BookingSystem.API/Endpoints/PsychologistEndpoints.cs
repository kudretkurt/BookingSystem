using BookingSystem.API.Extensions;
using BookingSystem.Application.Patient.CancelAppointment;
using BookingSystem.Application.Psychologist.Create;
using BookingSystem.Application.Psychologist.CreateAvailability;
using BookingSystem.Application.Psychologist.EditAvailability;
using BookingSystem.Application.Psychologist.GetById;
using MediatR;

namespace BookingSystem.API.Endpoints;

public static class PsychologistEndpoints
{
    public static void MapPsychologistEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/psychologists",
            async (CreatePsychologistCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

        app.MapPatch("api/psychologists/create-availability",
            async (CreateAvailabilityCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

        app.MapPatch("api/psychologists/edit-availability",
            async (EditAvailabilityCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });

        app.MapDelete("api/psychologists/cancel-appointment/{appointmentId}", async (Guid appointmentId,
            ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CancelAppointmentCommand(appointmentId), cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapGet("api/psychologists/{psychologistId}",
            async (Guid psychologistId, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetPsychologistByIdQuery(psychologistId), cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            });
    }
}