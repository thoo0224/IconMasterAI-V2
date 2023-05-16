using IconMasterAI.Application.Abstractions.Messaging;
using IconMasterAI.Core.Common;
using IconMasterAI.Core.Entities.Dto;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services;
using IconMasterAI.Core.Shared;

namespace IconMasterAI.Application.Users.Queries;

internal sealed class GetUserByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMappingService _mappingService;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMappingService mappingService)
    {
        _userRepository = userRepository;
        _mappingService = mappingService;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByIdAsync(request.Id).ConfigureAwait(false);
        if (user == null)
        {
            return Result.Failure<UserDto>(DomainErrors.Users.UserNotFound);
        }

        var dto = _mappingService.MapToDto(user);
        return Result.Success(dto);
    }
}
