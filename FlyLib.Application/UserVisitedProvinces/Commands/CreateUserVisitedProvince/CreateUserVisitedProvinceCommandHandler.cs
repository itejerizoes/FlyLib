using AutoMapper;
using FlyLib.Application.UserVisitedProvinces.DTOs;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using MediatR;

namespace FlyLib.Application.UserVisitedProvinces.Commands.CreateUserVisitedProvince
{
    public class CreateUserVisitedProvinceCommandHandler : IRequestHandler<CreateUserVisitedProvinceCommand, UserVisitedProvinceDto>
    {
        private readonly IUserVisitedProvinceRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateUserVisitedProvinceCommandHandler(IUserVisitedProvinceRepository repo, IUnitOfWork uow, IMapper mapper)
            => (_repo, _uow, _mapper) = (repo, uow, mapper);

        public async Task<UserVisitedProvinceDto> Handle(CreateUserVisitedProvinceCommand request, CancellationToken ct)
        {
            var entity = new UserVisitedProvince
            {
                UserId = request.UserId,
                ProvinceId = request.ProvinceId,
                VisitPhotos = request.VisitPhotos,
            };
            await _repo.AddAsync(entity, ct);
            await _uow.SaveChangesAsync(ct);
            return _mapper.Map<UserVisitedProvinceDto>(entity);
        }
    }
}
