using AutoMapper;
using FlyLib.Application.Users.DTOs;
using FlyLib.Domain.Abstractions;
using MediatR;

namespace FlyLib.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository repo, IMapper mapper)
            => (_repo, _mapper) = (repo, mapper);

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
        {
            var items = await _repo.GetAllAsync(ct);
            return items.Select(_mapper.Map<UserDto>);
        }
    }
}
