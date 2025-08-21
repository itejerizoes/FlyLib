using FlyLib.API.DTOs.v1.Photos.Responses;

namespace FlyLib.API.DTOs.v1.Visited.Responses
{
    public class VisitedResponseV1
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProvinceId { get; set; }
        public List<PhotoResponseV1> Photos { get; set; }

        public VisitedResponseV1() { }

        public VisitedResponseV1(int id, string userId, int provinceId, List<PhotoResponseV1> photos)
        {
            Id = id;
            UserId = userId;
            ProvinceId = provinceId;
            Photos = photos;
        }
    }
}
