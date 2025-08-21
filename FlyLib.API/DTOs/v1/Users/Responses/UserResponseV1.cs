using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.Users.Responses
{
    public class UserResponseV1
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string AuthProvider { get; set; }
        public List<VisitedResponseV1> Visiteds { get; set; }

        public UserResponseV1() { }

        public UserResponseV1(string id, string displayName, string authProvider, List<VisitedResponseV1> visiteds)
        {
            Id = id;
            DisplayName = displayName;
            AuthProvider = authProvider;
            Visiteds = visiteds;
        }
    }
}
