using FlyLib.API.DTOs.v1.Photos.Responses;

namespace FlyLib.API.DTOs.v1.Visited.Responses
{
    public record VisitedResponseV1(int Id, string UserId, int ProvinceId, ICollection<PhotoResponseV1> Photos);
}
