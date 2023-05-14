using IconMasterAI.Core.Shared;
using MediatR;

namespace IconMasterAI.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
