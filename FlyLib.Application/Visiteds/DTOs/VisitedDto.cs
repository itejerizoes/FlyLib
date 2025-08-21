using FlyLib.Application.Photos.DTOs;

namespace FlyLib.Application.Visiteds.DTOs
{
    public sealed class VisitedDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProvinceId { get; set; }
        public IEnumerable<PhotoDto> Photos { get; set; }

        public VisitedDto() { }

        public VisitedDto(int id, string userId, int provinceId, IEnumerable<PhotoDto> photos)
        {
            Id = id;
            UserId = userId;
            ProvinceId = provinceId;
            Photos = photos;
        }
    }
}
