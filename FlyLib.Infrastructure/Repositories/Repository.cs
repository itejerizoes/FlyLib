using FlyLib.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
            => await _db.ToListAsync(ct);

        public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
            => await _db.FindAsync([id], ct);

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _db.AddAsync(entity, ct);

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _db.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(object id, CancellationToken ct = default)
        {
            var entity = await _db.FindAsync([id], ct);
            if (entity is not null) _db.Remove(entity);
        }
    }
}
