namespace FlyLib.API.DTOs.v1.Photos.Requests
{
    public record UpdatePhotoRequestV1(int Id, string PhotoUrl, string? Description, int VisitedId);
}
