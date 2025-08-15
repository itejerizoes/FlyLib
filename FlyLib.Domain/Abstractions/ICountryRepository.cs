using FlyLib.Domain.Entities;

namespace FlyLib.Domain.Abstractions
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country?> GetByNameAsync(string name, CancellationToken ct = default);
    }
}
