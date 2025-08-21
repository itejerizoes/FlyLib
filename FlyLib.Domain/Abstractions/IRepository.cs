using System.Linq.Expressions;

namespace FlyLib.Domain.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default,
            params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(object id, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(object id, CancellationToken ct = default);
    }
}
