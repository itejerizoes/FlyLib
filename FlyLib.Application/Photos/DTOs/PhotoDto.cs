namespace FlyLib.Application.Photos.DTOs
{
    public sealed record PhotoDto(int Id, string PhotoUrl, string? Description, int VisitedId);
}
