using AutoMapper;
using FlyLib.Application.Visiteds.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.Visiteds.Commands.CreateVisited
{
    public class CreateVisitedCommandHandler : IRequestHandler<CreateVisitedCommand, VisitedDto>
    {
        private readonly IVisitedRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateVisitedCommandHandler(IVisitedRepository repo, IUnitOfWork uow, IMapper mapper)
            => (_repo, _uow, _mapper) = (repo, uow, mapper);

        public async Task<VisitedDto> Handle(CreateVisitedCommand request, CancellationToken ct)
        {
            var entity = new Visited(request.ProvinceId)
            {
                UserId = request.UserId,
                ProvinceId = request.ProvinceId,
                Photos = request.Photos,
            };
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return _mapper.Map<VisitedDto>(entity);
        }
    }
}
