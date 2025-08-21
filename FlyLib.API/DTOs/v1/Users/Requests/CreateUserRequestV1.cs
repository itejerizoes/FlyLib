namespace FlyLib.API.DTOs.v1.Users.Requests
{
    public class CreateUserRequestV1
    {
        public string DisplayName { get; set; }
        public string AuthProvider { get; set; }

        public CreateUserRequestV1() { }

        public CreateUserRequestV1(string displayName, string authProvider)
        {
            DisplayName = displayName;
            AuthProvider = authProvider;
        }
    }
}
