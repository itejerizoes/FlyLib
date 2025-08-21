
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TestFlyLibrary.Tests.Utilities;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string TestScheme = "TestScheme";

    private static IEnumerable<Claim> _testClaims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user"),
        new Claim(ClaimTypes.Email, "test@example.com"),
        new Claim(ClaimTypes.Name, "Test User"),
        new Claim(ClaimTypes.Role, "Admin")
    };

    public static IEnumerable<Claim> TestClaims
    {
        get => _testClaims;
        set => _testClaims = value ?? new List<Claim>();
    }

    public static void ResetClaims()
    {
        _testClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "Admin")
        };
    }

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                           ILoggerFactory logger,
                           UrlEncoder encoder,
                           ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = _testClaims?.ToList() ?? new List<Claim>();
        var identity = new ClaimsIdentity(claims, TestScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, TestScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
