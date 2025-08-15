namespace FlyLib.Application.VisitPhotos.DTOs
{
    public sealed record VisitPhotoDto(int Id, string PhotoUrl, string? Description, int UserVisitedProvinceId);
}
