namespace IconMasterAI.Core.Entities.Dto;

public class UserDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string? AvatarUrl { get; set; }
    public int Credits { get; set; } = 0;
}
