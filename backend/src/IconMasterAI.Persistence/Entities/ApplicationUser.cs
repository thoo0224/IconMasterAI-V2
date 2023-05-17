using Microsoft.AspNetCore.Identity;

namespace IconMasterAI.Persistence.Entities;

internal class ApplicationUser : IdentityUser
{
    public string? AvatarUrl { get; set; }
    public int Credits { get; set; } = 0;
}
