using FlyLib.API.DTOs.v1.ManageUser;
using FlyLib.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class ManageUsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new
                {
                    user.Id,
                    user.Email,
                    user.DisplayName,
                    Roles = roles
                });
            }

            return Ok(result);
        }

        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound($"Usuario con id '{email}' no encontrado.");

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.Email,
                user.DisplayName,
                Roles = roles
            });
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpGet("role/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound($"Rol con id '{id}' no encontrado.");

            return Ok(new
            {
                role.Id,
                role.Name,
                role.NormalizedName
            });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestV1 request)
        {
            if (string.IsNullOrWhiteSpace(request.UserEmail) || string.IsNullOrWhiteSpace(request.Role))
                return BadRequest("Email y rol son requeridos.");

            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            if (user == null)
                return NotFound($"Usuario con email '{request.UserEmail}' no encontrado.");

            if (!await _roleManager.RoleExistsAsync(request.Role))
                return BadRequest($"El rol '{request.Role}' no existe.");

            if (await _userManager.IsInRoleAsync(user, request.Role))
                return BadRequest($"El usuario ya tiene el rol '{request.Role}'.");

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => new { code = e.Code, description = e.Description })
                });

            return Ok(new { message = $"Rol '{request.Role}' asignado a '{request.UserEmail}' correctamente." });
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleRequestV1 request)
        {
            if (string.IsNullOrWhiteSpace(request.UserEmail) || string.IsNullOrWhiteSpace(request.Role))
                return BadRequest("Email y rol son requeridos.");

            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            if (user == null)
                return NotFound($"Usuario con email '{request.UserEmail}' no encontrado.");

            if (!await _roleManager.RoleExistsAsync(request.Role))
                return BadRequest($"El rol '{request.Role}' no existe.");

            if (!await _userManager.IsInRoleAsync(user, request.Role))
                return BadRequest($"El usuario no tiene el rol '{request.Role}'.");

            var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
            if (!result.Succeeded)
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => new { code = e.Code, description = e.Description })
                });

            return Ok(new { message = $"Rol '{request.Role}' removido de '{request.UserEmail}' correctamente." });
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestV1 request)
        {
            if (string.IsNullOrWhiteSpace(request.Role))
                return BadRequest("El nombre del rol es requerido.");

            if (await _roleManager.RoleExistsAsync(request.Role))
                return BadRequest($"El rol '{request.Role}' ya existe.");

            var result = await _roleManager.CreateAsync(new IdentityRole(request.Role));
            if (!result.Succeeded)
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => new { code = e.Code, description = e.Description })
                });

            return Ok(new { message = $"Rol '{request.Role}' creado correctamente." });
        }

        [HttpPost("delete-role")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleRequestV1 request)
        {
            if (string.IsNullOrWhiteSpace(request.Role))
                return BadRequest("El nombre del rol es requerido.");

            var role = await _roleManager.FindByNameAsync(request.Role);
            if (role == null)
                return NotFound($"El rol '{request.Role}' no existe.");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => new { code = e.Code, description = e.Description })
                });

            return Ok(new { message = $"Rol '{request.Role}' eliminado correctamente." });
        }
    }
}
