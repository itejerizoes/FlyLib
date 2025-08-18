using AutoMapper;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.DeleteUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.Users.Queries.GetAllUsers;
using FlyLib.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todos los usuarios.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(_mapper.Map<IEnumerable<UserResponseV1>>(result));
        }

        /// <summary>
        /// Obtener un usuario por su Id.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:string}")]
        [ProducesResponseType(typeof(UserResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<UserResponseV1>(result));
        }

        /// <summary>
        /// Crear un nuevo usuario.
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestV1 request)
        {
            var cmd = _mapper.Map<CreateUserCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<UserResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        /// <summary>
        /// Actualizar un usuario existente.
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id:string}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequestV1 request)
        {
            if (id != request.Id) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdateUserCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar un usuario.
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id:string}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }
    }
}
