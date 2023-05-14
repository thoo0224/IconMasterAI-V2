using IconMasterAI.Core.Shared;

using MediatR;

namespace IconMasterAI.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> 
    : IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{
}
