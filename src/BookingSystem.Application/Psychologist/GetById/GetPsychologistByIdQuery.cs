using BookingSystem.Application.Messaging;

namespace BookingSystem.Application.Psychologist.GetById;

public sealed record GetPsychologistByIdQuery(Guid PsychologistId) : IQuery<PsychologistResponse>;