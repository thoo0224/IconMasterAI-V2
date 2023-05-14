using IconMasterAI.Core.Shared;

namespace IconMasterAI.Core.Errors;

public static class DomainErrors
{
    public static class Users
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "That email is already in use.");

        public static readonly Error UserNameAlreadyInUse = new(
            "User.UserNameAlreadyInUse",
            "This user name is already in use.");

        public static readonly Error IdentityError = new(
            "User.IdentityError",
            "Invalid registration.");

        public static readonly Error InvalidEmailOrPassword = new(
            "User.InvalidEmailOrPassword",
            "Invalid email or password.");

        public static readonly Error GoogleLoginFailed = new(
            "User.GoogleLoginFailed",
            "Google login failed.");

        public static readonly Error UserNotFound = new(
            "User.NotFound",
            "User was not found.");
    }
}
