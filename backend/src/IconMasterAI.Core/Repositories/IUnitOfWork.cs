namespace IconMasterAI.Core.Repositories;

public interface IUnitOfWork
{
    Task SaveChangedAsync(CancellationToken ct = default);
}