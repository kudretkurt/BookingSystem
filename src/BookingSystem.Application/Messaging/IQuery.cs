using BookingSystem.Shared;
using MediatR;

namespace BookingSystem.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}