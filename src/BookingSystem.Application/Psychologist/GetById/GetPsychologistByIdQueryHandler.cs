using BookingSystem.Application.Messaging;
using BookingSystem.Domain.Psychologists;
using BookingSystem.Shared;
using Mapster;

namespace BookingSystem.Application.Psychologist.GetById;

public class GetPsychologistByIdQueryHandler(IPsychologistRepository psychologistRepository)
    : IQueryHandler<GetPsychologistByIdQuery, PsychologistResponse>
{
    public async Task<Result<PsychologistResponse>> Handle(GetPsychologistByIdQuery request,
        CancellationToken cancellationToken)
    {
        var psychologist =
            await psychologistRepository.GetPsychologistByIdAsync(request.PsychologistId, cancellationToken);
        if (psychologist == null)
            return Result.Failure<PsychologistResponse>(
                PsychologistErrors.PsychologistNotExists(request.PsychologistId));

        var psychologistResponse = psychologist.Adapt<PsychologistResponse>();
        return Result.Success(psychologistResponse);
    }
}