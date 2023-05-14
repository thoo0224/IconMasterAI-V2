using AutoMapper;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Entities.Dto;
using IconMasterAI.Core.Services;

namespace IconMasterAI.Persistence.Services;

internal sealed class MappingService : IMappingService
{
    private readonly IMapper _mapper;

    public MappingService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserDto MapToDto(User user)
        => _mapper.Map<UserDto>(user);
}
