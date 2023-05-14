using IconMasterAI.Core.Results;

namespace IconMasterAI.Core.Services.Security;

public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateLocalAsync(string email, string password);
    Task<AuthenticationResult> AuthenticateWithGoogleAsync(string accessToken);
}
