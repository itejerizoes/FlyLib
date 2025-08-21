namespace FlyLib.Application.Photos.DTOs
{
    public sealed record PhotoDto(int PhotoId, string Url, string? Description, int VisitedId);
}
