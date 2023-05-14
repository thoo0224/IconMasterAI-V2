using AutoMapper;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Entities.Dto;
using IconMasterAI.Persistence.Entities;

namespace IconMasterAI.Persistence.Mappings;

internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ApplicationUser>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}
