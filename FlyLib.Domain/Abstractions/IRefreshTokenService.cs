using FlyLib.Domain.Entities;

namespace FlyLib.Domain.Abstractions
{
    public interface IRefreshTokenService
    {
        RefreshToken GenerateToken(string userId);
        Task<RefreshToken?> GetValidTokenAsync(string token);
        Task RevokeTokenAsync(RefreshToken token, string? replacedBy = null);
    }
}
