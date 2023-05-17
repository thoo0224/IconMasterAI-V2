using System.Security.Claims;

using IconMasterAI.Core.Entities;

namespace IconMasterAI.Core.Services.Security;

public interface IUserAccessorService
{
    ClaimsPrincipal Claims { get; }

    Task<User?> GetUserAsync(CancellationToken ct = default);
}
