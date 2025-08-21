using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Entities;

namespace FlyLib.Application.Users.DTOs
{
    public sealed class UserDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string AuthProvider { get; set; }
        public IEnumerable<VisitedDto> Visiteds { get; set; }
        public IEnumerable<RefreshToken> RefreshTokens { get; set; }

        public UserDto() { }

        public UserDto(string id, string displayName, string authProvider, IEnumerable<VisitedDto> visiteds, IEnumerable<RefreshToken> refreshTokens)
        {
            Id = id;
            DisplayName = displayName;
            AuthProvider = authProvider;
            Visiteds = visiteds;
            RefreshTokens = refreshTokens;
        }
    }
}
