using FlyLib.Domain.Entities;

namespace FlyLib.Domain.Abstractions
{
    public interface IProvinceRepository : IRepository<Province>
    {
        Task<Province?> GetByNameAsync(string name, CancellationToken ct = default);
    }
}
