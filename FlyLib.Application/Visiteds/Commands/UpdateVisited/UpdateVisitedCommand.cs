using FlyLib.Application.Photos.DTOs;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.UpdateVisited
{
    public class UpdateVisitedCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProvinceId { get; set; }
        public IEnumerable<PhotoDto> Photos { get; set; }

        public UpdateVisitedCommand() { }

        public UpdateVisitedCommand(int id, string userId, int provinceId, IEnumerable<PhotoDto> photos)
        {
            Id = id;
            UserId = userId;
            ProvinceId = provinceId;
            Photos = photos;
        }
    }
}
