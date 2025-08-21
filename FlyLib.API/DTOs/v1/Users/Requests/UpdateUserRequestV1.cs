namespace FlyLib.API.DTOs.v1.Users.Requests
{
    public class UpdateUserRequestV1
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string AuthProvider { get; set; }

        public UpdateUserRequestV1() { }

        public UpdateUserRequestV1(string id, string displayName, string authProvider)
        {
            Id = id;
            DisplayName = displayName;
            AuthProvider = authProvider;
        }
    }
}
