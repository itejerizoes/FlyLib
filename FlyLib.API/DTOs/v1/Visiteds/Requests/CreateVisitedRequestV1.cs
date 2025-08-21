using FlyLib.API.DTOs.v1.Photos.Requests;

namespace FlyLib.API.DTOs.v1.Visited.Requests
{
    public class CreateVisitedRequestV1
    {
        public string UserId { get; set; }
        public int ProvinceId { get; set; }
        public List<CreatePhotoRequestV1> Photos { get; set; }

        public CreateVisitedRequestV1() { }

        public CreateVisitedRequestV1(string userId, int provinceId, List<CreatePhotoRequestV1> photos)
        {
            UserId = userId;
            ProvinceId = provinceId;
            Photos = photos;
        }
    }
}
