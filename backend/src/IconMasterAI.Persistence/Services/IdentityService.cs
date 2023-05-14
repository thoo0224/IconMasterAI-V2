using AutoMapper;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Services.Security;
using IconMasterAI.Persistence.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IconMasterAI.Persistence.Services;

internal sealed class IdentityService : IIdentityService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public IdentityService(
        SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> CheckPasswordSignInAsync(User user, string password)
    {
        var applicationUser = await _userManager.Users
            .Where(x => x.Email == user.Email)
            .FirstAsync().ConfigureAwait(false);

        var result = await _signInManager.CheckPasswordSignInAsync(applicationUser, password, false)
            .ConfigureAwait(false);

        return result.Succeeded;
    }

    public async Task<bool> CreateExternalUserAsync(User user, string provider, string subject)
    {
        var applicationUser = _mapper.Map<ApplicationUser>(user);
        var userLoginInfo = new UserLoginInfo(provider, subject, "Google");

        var result = await _userManager.CreateAsync(applicationUser).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            return false;
        }

        result = await _userManager.AddLoginAsync(applicationUser, userLoginInfo).ConfigureAwait(false);
        return result.Succeeded;
    }
}
