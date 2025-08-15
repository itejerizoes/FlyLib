using FlyLib.Application.Common.Constants;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.DeleteUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.Users.Queries.GetAllUsers;
using FlyLib.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetAllUsersQuery()));

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand cmd)
        {
            var created = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserCommand cmd)
        {
            if (id != cmd.Id) return BadRequest("Route id and body id must match");
            await _mediator.Send(cmd);
            return NoContent();
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }
    }
}
