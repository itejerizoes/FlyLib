using FluentValidation;
using FluentValidation.AspNetCore;
using FlyLib.API.Middleware;
using FlyLib.Application.Common.Behaviors;
using FlyLib.Application.Mapping;
using FlyLib.Domain.Abstractions;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Infrastructure.Repositories;
using FlyLib.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlyLib.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFlyLibraryServices(this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            services.AddDbContext<FlyLibDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // UoW + Repos
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserVisitedProvinceRepository, UserVisitedProvinceRepository>();
            services.AddScoped<IVisitPhotoRepository, VisitPhotoRepository>();

            // MediatR + Behaviors
            services.AddMediatR(typeof(Program).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

            // Middleware
            services.AddTransient<GlobalExceptionMiddleware>();

            // Auth (si corresponde): AddAuthentication().AddJwtBearer(...) + Roles

            return services;
        }
    }
}
