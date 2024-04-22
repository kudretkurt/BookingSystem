using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;
using Mapster;

namespace BookingSystem.Application.Psychologist.CreateAvailability;

public class CreateAvailabilityCommandHandler(IPsychologistRepository psychologistRepository)
    : ICommandHandler<CreateAvailabilityCommand, PsychologistResponse>
{
    public async Task<Result<PsychologistResponse>> Handle(CreateAvailabilityCommand command,
        CancellationToken cancellationToken)
    {
        var psychologist =
            await psychologistRepository.GetPsychologistByIdAsync(command.PsychologistId, cancellationToken);

        if (psychologist == null)
            return Result.Failure<PsychologistResponse>(
                PsychologistErrors.PsychologistNotExists(command.PsychologistId));

        var availabilityResult =
            psychologist.CreateAvailability(command.From, command.To,
                command.StartTime,
                command.EndTime);
        if (availabilityResult.IsFailure) return Result.Failure<PsychologistResponse>(availabilityResult.Error);

        await psychologistRepository.UpdateAsync(psychologist, cancellationToken);

        var psychologistResponse = psychologist.Adapt<PsychologistResponse>();

        return Result.Success(psychologistResponse);
    }
}