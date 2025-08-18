using FlyLib.API.DTOs.v1.Photos.Requests;

namespace FlyLib.API.DTOs.v1.Visited.Requests
{
    public record CreateVisitedRequestV1(string UserId, int ProvinceId, string? Description, DateTime VisitedAt, ICollection<CreatePhotoRequestV1> Photos);
}
