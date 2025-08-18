using FlyLib.API.Extensions;
using FlyLib.Infrastructure.Persistence;
using FlyLib.Tests.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                // Remover DbContext original
                // =========================
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<FlyLibDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // =========================
                // Registrar DbContext InMemory
                // =========================
                services.AddDbContext<FlyLibDbContext>(options =>
                    options.UseInMemoryDatabase("FlyLibTestDb"));

                // =========================
                // Remover autenticación original (Identity / Jwt)
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
                // Registrar servicios de la librería
                // =========================
                services.AddFlyLibraryServices(new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build(), useInMemory: true);

                // =========================
                // Construir ServiceProvider temporal y semilla de datos
                // =========================
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<FlyLibDbContext>();

                    // Aseguramos la base
                    db.Database.EnsureDeleted(); // limpia cada test
                    db.Database.EnsureCreated();

                    // Inicializar datos
                    SeedData.Initialize(db);
                }
            });
        }
    }
}