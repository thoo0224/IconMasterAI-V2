using System.Security.Claims;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services.Security;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IconMasterAI.Infrastructure.Services.Security;

internal sealed class UserAccessorService : IUserAccessorService
{
    public ClaimsPrincipal Claims => _httpContextAccessor.HttpContext!.User;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public UserAccessorService(
        IHttpContextAccessor httpContextAccessor, 
        IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserAsync(CancellationToken ct = default)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var userId = Claims.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return null;
        }

        var userRepository = httpContext.RequestServices.GetRequiredService<IUserRepository>();
        var user = await userRepository.FindUserByIdAsync(userId)
            .ConfigureAwait(false);

        return user;
    }
}
