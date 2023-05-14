namespace IconMasterAI.Core.Entities;

public sealed class User
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string? AvatarUrl { get; set; }

    public static User Create(
        string userName,
        string email,
        string? avatarUrl = null,
        string? id = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid().ToString(),
            AvatarUrl = avatarUrl,
            UserName = userName,
            Email = email
        };
    }
}