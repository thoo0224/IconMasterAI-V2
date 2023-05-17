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

    public static class Generator
    {
        public static readonly Error SomethingWentWrong = new(
            "Generator.Unauthorized",
            "Something went wrong.");

        public static readonly Error NotEnoughCredits = new(
            "Generator.NotEnoughCredits",
            "You don't have enough credits for this action.");
    }

    public static class Global
    {
        public static readonly Error Unauthorized = new(
            "Global.Unauthorized",
            "Unauthorized.");
    }
}
