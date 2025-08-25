namespace FlyLib.API.DTOs.v1.Auth.Request
{
    public class RegisterRequestV1
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
