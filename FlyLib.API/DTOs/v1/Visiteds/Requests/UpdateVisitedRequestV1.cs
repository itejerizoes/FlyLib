using FlyLib.API.DTOs.v1.Photos.Requests;

namespace FlyLib.API.DTOs.v1.Visited.Requests
{
    public class UpdateVisitedRequestV1
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProvinceId { get; set; }
        public List<UpdatePhotoRequestV1> Photos { get; set; }

        public UpdateVisitedRequestV1() { }

        public UpdateVisitedRequestV1(int id, string userId, int provinceId, List<UpdatePhotoRequestV1> photos)
        {
            Id = id;
            UserId = userId;
            ProvinceId = provinceId;
            Photos = photos;
        }
    }
}
