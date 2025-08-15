using AutoMapper;
using FlyLib.Application.UserVisitedProvinces.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Queries.GetUserVisitedProvinceById
{
    public class GetUserVisitedProvinceByIdQueryHandler : IRequestHandler<GetUserVisitedProvinceByIdQuery, UserVisitedProvinceDto?>
    {
        private readonly IUserVisitedProvinceRepository _repo;
        private readonly IMapper _mapper;

        public GetUserVisitedProvinceByIdQueryHandler(IUserVisitedProvinceRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<UserVisitedProvinceDto?> Handle(GetUserVisitedProvinceByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : _mapper.Map<UserVisitedProvinceDto>(entity);
        }
    }
}
