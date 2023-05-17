using System.Linq.Expressions;

using AutoMapper;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Results;
using IconMasterAI.Persistence.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IconMasterAI.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<CreateUserResult> CreateAsync(User user, string password)
    {
        var appUser = new ApplicationUser
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            UserName = user.UserName
        };

        var result = await _userManager.CreateAsync(appUser, password)
            .ConfigureAwait(false);

        return new CreateUserResult(
            result.Succeeded,
            result.Errors
                .Select(x => new CreateUserError(x.Code, x.Description))
                .ToArray());
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        var dbUser = await FindUserAsync(u => u.Email == email)
            .ConfigureAwait(false);

        if (dbUser == null)
            return null;

        return  _mapper.Map<User>(dbUser);
    }

    public async Task<User?> FindUserByIdAsync(string id)
    {
        var dbUser = await FindUserAsync(u => u.Id == id)
            .ConfigureAwait(false);

        if (dbUser == null)
            return null;

        return _mapper.Map<User>(dbUser);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return await _userManager.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync().ConfigureAwait(false) == null;
    }

    public async Task<bool> IsUserNameUniqueAsync(string userName)
    {
        return await _userManager.Users
            .Where(u => u.UserName == userName)
            .FirstOrDefaultAsync().ConfigureAwait(false) == null;
    }

    public async Task<bool> HasEnoughCreditsAsync(User user, int credits, CancellationToken ct = default)
    {
        var result = await _userManager.Users
            .Where(u => u.Id.Equals(user.Id))
            .FirstOrDefaultAsync(cancellationToken: ct).ConfigureAwait(false);

        if (result == null)
        {
            return false;
        }

        return result.Credits >= credits;
    }

    public async Task RemoveCreditsAsync(User user, int credits, CancellationToken ct = default)
    {
        var dbUser = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id.Equals(user.Id), cancellationToken: ct)
            .ConfigureAwait(false);

        if (dbUser == null)
        {
            return;
        }

        dbUser.Credits -= credits;
        if (dbUser.Credits <= 0)
        {
            dbUser.Credits = 0;
        }
    }

    private async Task<ApplicationUser?> FindUserAsync(Expression<Func<ApplicationUser, bool>> predicate)
    {
        return await _userManager.Users
            .Where(predicate)
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }
}
