using FlyLib.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FlyLib.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _db;

        public Repository(DbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.ToListAsync(ct);
        }

        public virtual async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
            => await _db.FindAsync(new object[] { id }, ct);

        public virtual async Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db.AsNoTracking();
            query = query.Where(predicate);

            if (includes.Any())
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(T entity, CancellationToken ct = default)
            => await _db.AddAsync(entity, ct);

        public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(object id, CancellationToken ct = default)
        {
            var entity = await _db.FindAsync(new object[] { id }, ct);
            if (entity != null)
            {
                _db.Remove(entity);
            }
        }
    }
}
