namespace FlyLib.API.DTOs.v1.ManageUser
{
    public class AssignRoleRequestV1
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
