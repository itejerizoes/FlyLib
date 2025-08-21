using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Entities;

namespace FlyLib.Application.Users.DTOs
{
    public sealed record UserDto(string Id, string DisplayName, string AuthProvider, IEnumerable<VisitedDto> Visiteds, IEnumerable<RefreshToken> RefreshTokens);
}
