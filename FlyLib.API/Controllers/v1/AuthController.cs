using FlyLib.API.DTOs.v1.Auth.Request;
using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using FlyLib.Infrastructure.Identity.Jwt;
using FlyLib.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly FlyLibDbContext _db;

        public AuthController(
            UserManager<User> userManager,
            ITokenService tokenService,
            RefreshTokenService refreshTokenService,
            FlyLibDbContext db)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _db = db;
        }

        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string provider, [FromQuery] string returnUrl = "/")
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromQuery] string returnUrl = "/")
        {
            var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!info.Succeeded)
                return Unauthorized();

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            // Buscar o crear usuario
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User(name)
                {
                    UserName = email,
                    Email = email,
                    DisplayName = name,
                    AuthProvider = info.Properties.Items[".AuthScheme"]
                };
                await _userManager.CreateAsync(user);
            }

            // Asignar el rol "User" al usuario recién creado
            await _userManager.AddToRoleAsync(user, "User");

            // Emitir JWT propio
            var jwt = await _tokenService.CreateToken(user);

            // Emitir refresh token
            var refreshToken = _refreshTokenService.GenerateToken(user.Id);
            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();

            // Limpiar el login externo
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var roles = await _userManager.GetRolesAsync(user);

            // Construye la URL de redirección al frontend
            var frontendUrl = $"http://localhost:3000{returnUrl}?token={jwt}&refreshToken={refreshToken.Token}&displayName={user.DisplayName}&roles={string.Join(",", roles)}";
            return Redirect(frontendUrl);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized();

            var jwt = await _tokenService.CreateToken(user);
            var refreshToken = _refreshTokenService.GenerateToken(user.Id);
            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token = jwt,
                refreshToken = refreshToken.Token,
                displayName = user.DisplayName,
                roles
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestV1 request)
        {
            var oldToken = await _refreshTokenService.GetValidTokenAsync(request.RefreshToken);
            if (oldToken == null)
                return Unauthorized();

            var user = oldToken.User!;
            var jwt = await _tokenService.CreateToken(user);

            // Rotación segura: revoca el anterior y emite uno nuevo
            var newRefreshToken = _refreshTokenService.GenerateToken(user.Id);
            await _refreshTokenService.RevokeTokenAsync(oldToken, newRefreshToken.Token);
            _db.RefreshTokens.Add(newRefreshToken);
            await _db.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token = jwt,
                refreshToken = newRefreshToken.Token,
                displayName = user.DisplayName,
                roles
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestV1 request)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email y contraseña son requeridos.");

            var user = new User(request.DisplayName)
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName,
                AuthProvider = "Email"
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(new
                    {
                        errors = result.Errors.Select(e => new { code = e.Code, description = e.Description })
                    });

            // Asignar el rol "User" al usuario recién creado
            await _userManager.AddToRoleAsync(user, "User");

            // Emitir JWT propio
            var jwt = await _tokenService.CreateToken(user);

            // Emitir refresh token
            var refreshToken = _refreshTokenService.GenerateToken(user.Id);
            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token = jwt,
                refreshToken = refreshToken.Token,
                displayName = user.DisplayName,
                roles
            });
        }
    }
}
