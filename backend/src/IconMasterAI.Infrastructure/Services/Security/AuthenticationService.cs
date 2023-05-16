using Google.Apis.Auth;

using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Errors;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Results;
using IconMasterAI.Core.Services.Security;

namespace IconMasterAI.Infrastructure.Services.Security;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthenticationService(
        IIdentityService identityService, 
        IUserRepository userRepository, 
        IJwtService jwtService)
    {
        _identityService = identityService;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthenticationResult> AuthenticateLocalAsync(string email, string password)
    {
        var user = await _userRepository.FindUserByEmailAsync(email);
        if (user == null)
        {
            return AuthenticationResult.Failure(DomainErrors.Users.InvalidEmailOrPassword);
        }

        if (!await _identityService.CheckPasswordSignInAsync(user, password))
        {
            return AuthenticationResult.Failure(DomainErrors.Users.InvalidEmailOrPassword);
        }

        var token = _jwtService.CreateToken(user);
        return AuthenticationResult.Success(token);
    }

    public async Task<AuthenticationResult> AuthenticateWithGoogleAsync(string accessToken)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(accessToken).ConfigureAwait(false);
            if (payload == null)
            {
                return AuthenticationResult.Failure(DomainErrors.Users.GoogleLoginFailed);
            }
        }
        catch
        {
            return AuthenticationResult.Failure(DomainErrors.Users.GoogleLoginFailed);
        }

        var user = await _userRepository.FindUserByEmailAsync(payload.Email);
        if (user == null)
        {
            user = User.Create(
                payload.Name,
                payload.Email,
                id: payload.Subject,
                avatarUrl: payload.Picture);

            var createUserResult = await _identityService.CreateExternalUserAsync(user, "Google", payload.Subject).ConfigureAwait(false);
            if (!createUserResult)
            {
                return AuthenticationResult.Failure(DomainErrors.Users.GoogleLoginFailed);
            }
        }

        var token = _jwtService.CreateToken(user);
        return AuthenticationResult.Success(token);
    }
}
