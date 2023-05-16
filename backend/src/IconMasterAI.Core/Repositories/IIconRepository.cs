using IconMasterAI.Core.Entities;

namespace IconMasterAI.Core.Repositories;

public interface IIconRepository
{
    Task<Icon[]> CreateManyAsync(string rawPrompt, string finalPrompt, string[] images, CancellationToken ct = default);
}
