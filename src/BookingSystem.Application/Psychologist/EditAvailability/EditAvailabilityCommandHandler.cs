using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;
using Mapster;

namespace BookingSystem.Application.Psychologist.EditAvailability;

public class EditAvailabilityCommandHandler(IPsychologistRepository psychologistRepository)
    : ICommandHandler<EditAvailabilityCommand, PsychologistResponse>
{
    public async Task<Result<PsychologistResponse>> Handle(EditAvailabilityCommand command,
        CancellationToken cancellationToken)
    {
        var psychologist =
            await psychologistRepository.GetPsychologistByIdAsync(command.PsychologistId, cancellationToken);
        if (psychologist == null)
            return Result.Failure<PsychologistResponse>(
                PsychologistErrors.PsychologistNotExists(command.PsychologistId));

        var editAvailabilityResult = psychologist.EditAvailability(command.Date, command.StartTime, command.EndTime,
            command.NewDate,
            command.NewStartTime, command.NewEndTime);
        if (editAvailabilityResult.IsFailure) return Result.Failure<PsychologistResponse>(editAvailabilityResult.Error);

        await psychologistRepository.UpdateAsync(psychologist, cancellationToken);
        var psychologistResponse = psychologist.Adapt<PsychologistResponse>();
        return Result.Success(psychologistResponse);
    }
}