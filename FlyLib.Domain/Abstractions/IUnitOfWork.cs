namespace FlyLib.Domain.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
