namespace FlyLib.API.DTOs.v1.VisitPhoto.Requests
{
    public record UpdateVisitPhotoRequestV1(int Id, string PhotoUrl, string? Description, int UserVisitedProvinceId);
}
