namespace FlyLib.API.DTOs.v1.Photos.Responses
{
    public record PhotoResponseV1(int Id, string PhotoUrl, string? Description, int VisitedId);
}
