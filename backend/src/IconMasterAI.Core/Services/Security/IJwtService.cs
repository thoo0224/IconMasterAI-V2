using IconMasterAI.Core.Entities;

namespace IconMasterAI.Core.Services.Security;

public interface IJwtService
{
    string CreateToken(User user);
}
