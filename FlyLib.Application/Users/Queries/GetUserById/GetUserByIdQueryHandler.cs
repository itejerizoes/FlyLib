using AutoMapper;
using FlyLib.Application.Users.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : _mapper.Map<UserDto>(entity);
        }
    }
}
