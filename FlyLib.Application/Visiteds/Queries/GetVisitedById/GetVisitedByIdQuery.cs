using FlyLib.Application.Visiteds.DTOs;
using MediatR;

namespace FlyLib.Application.Visiteds.Queries.GetVisitedById
{
    public class GetVisitedByIdQuery : IRequest<VisitedDto?>
    {
        public int Id { get; set; }

        public GetVisitedByIdQuery() { }

        public GetVisitedByIdQuery(int id)
        {
            Id = id;
        }
    }
}
