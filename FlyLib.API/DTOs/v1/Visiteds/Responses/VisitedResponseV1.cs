using FlyLib.Domain.Entities;

namespace FlyLib.API.DTOs.v1.Visited.Responses
{
    public record VisitedResponseV1(int Id, string UserId, int ProvinceId, ICollection<Domain.Entities.Photo> Photos);
}
