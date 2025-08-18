using FlyLib.Domain.Entities;

namespace FlyLib.Application.Visiteds.DTOs
{
    public sealed record VisitedDto(int Id, string UserId, int ProvinceId, ICollection<Photo> Photos);
}
