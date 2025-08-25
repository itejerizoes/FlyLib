using AutoMapper;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Visited.Requests;
using FlyLib.API.DTOs.v1.Visited.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Visiteds.Commands.CreateVisited;
using FlyLib.Application.Visiteds.Commands.DeleteVisited;
using FlyLib.Application.Visiteds.Commands.UpdateVisited;
using FlyLib.Application.Visiteds.Queries.GetAllVisiteds;
using FlyLib.Application.Visiteds.Queries.GetVisitedById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VisitedsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public VisitedsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todos los usuario visitado provincias.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VisitedResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllVisitedsQuery());
            return Ok(_mapper.Map<IEnumerable<VisitedResponseV1>>(result));
        }


        /// <summary>
        /// Obtener un usuarios visitado provincia por Id.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VisitedResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetVisitedByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<VisitedResponseV1>(result));
        }

        /// <summary>
        /// Registrar un nuevo usuarios visitado provincia.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        [ProducesResponseType(typeof(VisitedResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateVisitedRequestV1 request)
        {
            var cmd = _mapper.Map<CreateVisitedCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<VisitedResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Actualizar un usuarios visitado provincia.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVisitedRequestV1 request)
        {
            if (id != request.Id) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdateVisitedCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar un usuarios visitado provincia.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteVisitedCommand(id));
            return NoContent();
        }
    }
}
