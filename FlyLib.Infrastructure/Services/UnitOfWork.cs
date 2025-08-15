using FlyLib.Domain.Abstractions;
using FlyLib.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyLib.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlyLibDbContext _context;
        public UnitOfWork(FlyLibDbContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);

        public ValueTask DisposeAsync() => _context.DisposeAsync();
    }
}
