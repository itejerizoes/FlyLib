namespace FlyLib.API.DTOs.v1.Users.Requests
{
    public record UpdateUserRequestV1(string Id, string DisplayName, string AuthProvider);
}
