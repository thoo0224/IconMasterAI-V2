using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Entities.Dto;

namespace IconMasterAI.Application.Users.Queries;

public sealed record GetUserByIdQuery(
    string Id) : IQuery<UserDto>;
