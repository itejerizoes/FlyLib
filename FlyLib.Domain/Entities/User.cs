using Microsoft.AspNetCore.Identity;

namespace FlyLib.Domain.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public string AuthProvider { get; set; } = string.Empty;
        public ICollection<UserVisitedProvince> UserVisitedProvinces { get; set; } = new List<UserVisitedProvince>();
    }
}
