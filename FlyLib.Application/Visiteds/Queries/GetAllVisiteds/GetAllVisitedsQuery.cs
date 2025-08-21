using FlyLib.Application.Visiteds.DTOs;
using MediatR;

namespace FlyLib.Application.Visiteds.Queries.GetAllVisiteds
{
    public class GetAllVisitedsQuery : IRequest<IEnumerable<VisitedDto>>
    {
        public GetAllVisitedsQuery() { }
    }
}
