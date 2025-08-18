namespace FlyLib.API.DTOs.v1.VisitPhoto.Requests
{
    public record CreateVisitPhotoRequestV1(string PhotoUrl, string? Description, int UserVisitedProvinceId);
}
