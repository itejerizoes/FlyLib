using FlyLib.Domain.Entities;

namespace FlyLib.Domain.Abstractions
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
