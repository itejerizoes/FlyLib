using FlyLib.Application.Photos.DTOs;

namespace FlyLib.Application.Visiteds.DTOs
{
    public sealed record VisitedDto(int Id, string UserId, int ProvinceId, IEnumerable<PhotoDto> Photos);
}
