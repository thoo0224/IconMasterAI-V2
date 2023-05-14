using IconMasterAI.Core.Entities;

namespace IconMasterAI.Core.Services.Security;

public interface IIdentityService
{
    Task<bool> CheckPasswordSignInAsync(User user, string password);
    Task<bool> CreateExternalUserAsync(User user, string provider, string subject);
}
