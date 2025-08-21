using FlyLib.API.Extensions;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Tests.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace TestFlyLibrary.Tests.Utilities
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");


            builder.ConfigureServices(services =>
            {
                // =========================
                // Reemplazar DbContext por InMemory con nombre único
                // =========================
                var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FlyLibDbContext>));
                if (dbDescriptor != null) services.Remove(dbDescriptor);

                var dbName = Guid.NewGuid().ToString();
                services.AddDbContext<FlyLibDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));

                services.AddFlyLibraryServices(new ConfigurationBuilder().Build(), useInMemory: true);

                // =========================
                // Remover autenticación original
                // =========================
                var authDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IAuthenticationSchemeProvider));
                if (authDescriptor != null)
                    services.Remove(authDescriptor);

                // =========================
                // Configurar autenticación de test
                // =========================
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.TestScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.TestScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.TestScheme, options => { });

                // =========================
                // Semilla de datos
                // =========================
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                SeedData.Initialize(db);
            });
        }
    }
}