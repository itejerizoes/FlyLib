using AutoMapper;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using FlyLib.API.DTOs.v1.Provinces.Responses;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using FlyLib.API.DTOs.v1.UserVisitedProvince.Requests;
using FlyLib.API.DTOs.v1.UserVisitedProvince.Responses;
using FlyLib.API.DTOs.v1.VisitPhoto.Requests;
using FlyLib.API.DTOs.v1.VisitPhoto.Responses;
using FlyLib.Application.Countries.Commands.CreateCountry;
using FlyLib.Application.Countries.Commands.UpdateCountry;
using FlyLib.Application.Provinces.Commands.CreateProvince;
using FlyLib.Application.Provinces.Commands.UpdateProvince;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.UserVisitedProvinces.Commands.CreateUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Commands.UpdateUserVisitedProvince;
using FlyLib.Application.VisitPhotos.Commands.CreateVisitPhoto;
using FlyLib.Application.VisitPhotos.Commands.UpdateVisitPhoto;
using FlyLib.Domain.Entities;

namespace FlyLib.API.Mappings.v1
{
    public class MappingProfileV1 : Profile
    {
        public MappingProfileV1()
        {

            // Request → Commands
            CreateMap<CreateCountryRequestV1, CreateCountryCommand>();
            CreateMap<UpdateCountryRequestV1, UpdateCountryCommand>();
            CreateMap<CreateProvinceRequestV1, CreateProvinceCommand>();
            CreateMap<UpdateProvinceRequestV1, UpdateProvinceCommand>();
            CreateMap<CreateUserRequestV1, CreateUserCommand>();
            CreateMap<UpdateUserRequestV1, UpdateUserCommand>();
            CreateMap<CreateUserVisitedProvinceRequestV1, CreateUserVisitedProvinceCommand>();
            CreateMap<UpdateUserVisitedProvinceRequestV1, UpdateUserVisitedProvinceCommand>();
            CreateMap<CreateVisitPhotoRequestV1, CreateVisitPhotoCommand>();
            CreateMap<UpdateVisitPhotoRequestV1, UpdateVisitPhotoCommand>();

            // Domain → Response
            CreateMap<Country, CountryResponseV1>();
            CreateMap<Province, ProvinceResponseV1>();
            CreateMap<User, UserResponseV1>();
            CreateMap<UserVisitedProvince, UserVisitedProvinceResponseV1>();
            CreateMap<VisitPhoto, VisitPhotoResponseV1>();
        }
    }
}
