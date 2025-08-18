using FluentValidation;
using FluentValidation.AspNetCore;
using FlyLib.API.Mappings.v1;
using FlyLib.API.Mappings.v2;
using FlyLib.API.Middleware;
using FlyLib.Application.Common.Behaviors;
using FlyLib.Application.Mapping;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Infrastructure.Repositories;
using FlyLib.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfileApplication>());
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfileV1>());
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfileV2>());

            //Versionado de la API
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV"; // ej: v1, v2
                opt.SubstituteApiVersionInUrl = true;
            });

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(MappingProfileV1).Assembly);

            // Middleware
            services.AddTransient<GlobalExceptionMiddleware>();

            // Auth (si corresponde): AddAuthentication().AddJwtBearer(...) + Roles
            // Configuración de Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<FlyLibDbContext>()
                .AddDefaultTokenProviders();

            var jwtKey = config["Jwt:Key"] ?? "thoifel_marlo_123+";
            var jwtIssuer = config["Jwt:Issuer"] ?? "TestFlyLibrary";
            var jwtAudience = config["Jwt:Audience"] ?? "TestFlyClient";

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                })
                .AddGoogle("Google", options =>
                {
                    options.ClientId = config["Authentication:Google:ClientId"]!;
                    options.ClientSecret = config["Authentication:Google:ClientSecret"]!;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddMicrosoftAccount("Microsoft", options =>
                {
                    options.ClientId = config["Authentication:Microsoft:ClientId"]!;
                    options.ClientSecret = config["Authentication:Microsoft:ClientSecret"]!;
                });

            return services;
        }
    }
}
