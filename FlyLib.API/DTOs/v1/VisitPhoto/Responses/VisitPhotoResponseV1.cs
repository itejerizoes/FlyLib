namespace FlyLib.API.DTOs.v1.VisitPhoto.Responses
{
    public record VisitPhotoResponseV1(int Id, string PhotoUrl, string? Description, int UserVisitedProvinceId);
}
