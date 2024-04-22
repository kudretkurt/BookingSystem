using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;

namespace BookingSystem.Application.Psychologist.Create;

internal sealed class CreatePsychologistCommandHandler(IPsychologistRepository psychologistRepository)
    : ICommandHandler<CreatePsychologistCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePsychologistCommand command, CancellationToken cancellationToken)
    {
        var psychologistResult = Domain.Psychologists.Psychologist.Create(command.FirstName, command.LastName);
        if (psychologistResult.IsFailure) return Result.Failure<Guid>(psychologistResult.Error);

        await psychologistRepository.Insert(psychologistResult.Value, cancellationToken);

        return psychologistResult.Value.Id;
    }
}