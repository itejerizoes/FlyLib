using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FlyLib.Infrastructure.Identity.Jwt
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly FlyLibDbContext _db;

        public RefreshTokenService(FlyLibDbContext db)
        {
            _db = db;
        }

        public RefreshToken GenerateToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return new RefreshToken
            {
                Token = token,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
        }

        public async Task<RefreshToken?> GetValidTokenAsync(string token)
        {
            return await _db.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow);
        }

        public async Task RevokeTokenAsync(RefreshToken token, string? replacedBy = null)
        {
            token.IsRevoked = true;
            token.ReplacedByToken = replacedBy;
            await _db.SaveChangesAsync();
        }
    }
}
