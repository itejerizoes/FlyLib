using Microsoft.AspNetCore.Identity;

namespace FlyLib.Domain.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public string AuthProvider { get; set; } = string.Empty;
        public ICollection<Visited> Visiteds { get; set; } = new List<Visited>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public User(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(userName));
            UserName = userName;
        }
    }
}
