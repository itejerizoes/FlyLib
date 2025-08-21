using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.Users.Responses
{
    public record UserResponseV1(string Id, string DisplayName, string AuthProvider, ICollection<VisitedResponseV1> Visiteds, ICollection<RefreshToken> RefreshTokens);
}
