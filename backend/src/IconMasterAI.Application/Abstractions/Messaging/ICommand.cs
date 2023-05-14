using IconMasterAI.Core.Shared;

using MediatR;

namespace IconMasterAI.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}