using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Entities.Dto;

namespace IconMasterAI.Core.Services;

public interface IMappingService
{
    UserDto MapToDto(User user);
}
