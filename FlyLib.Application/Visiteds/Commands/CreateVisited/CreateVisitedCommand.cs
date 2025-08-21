using FlyLib.Application.Photos.DTOs;
using FlyLib.Application.Visiteds.DTOs;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.CreateVisited
{
    public class CreateVisitedCommand : IRequest<VisitedDto>
    {
        public string UserId { get; set; }
        public int ProvinceId { get; set; }
        public IEnumerable<PhotoDto> Photos { get; set; }

        public CreateVisitedCommand() { }

        public CreateVisitedCommand(string userId, int provinceId, IEnumerable<PhotoDto> photos)
        {
            UserId = userId;
            ProvinceId = provinceId;
            Photos = photos;
        }
    }
}
