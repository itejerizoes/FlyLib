using FlyLib.Domain.Abstractions;
using FlyLib.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
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
                user = new User
                {
                    UserName = email,
                    Email = email,
                    DisplayName = name,
                    AuthProvider = info.Properties.Items[".AuthScheme"]
                };
                await _userManager.CreateAsync(user);
            }

            // Emitir JWT propio
            var jwt = await _tokenService.CreateToken(user);

            // Limpiar el login externo
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Devuelve el JWT (puedes devolverlo en el body, en un header, o redirigir con el token)
            return Ok(new { token = jwt, returnUrl });
        }
    }
}
