using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Results;

namespace IconMasterAI.Core.Repositories;

public interface IUserRepository
{
    Task<CreateUserResult> CreateAsync(User user, string password);
    Task<User?> FindUserByEmail(string email);
    Task<User?> FindUserById(string id);
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsUserNameUniqueAsync(string userName);
}
