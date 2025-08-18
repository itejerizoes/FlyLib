using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.Visited.Requests
{
    public record UpdateVisitedRequestV1(int Id, string UserId, int ProvinceId, ICollection<Domain.Entities.Photo> Photos);
}
