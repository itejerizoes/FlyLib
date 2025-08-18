using AutoMapper;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.UserVisitedProvince.Requests;
using FlyLib.API.DTOs.v1.UserVisitedProvince.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.UserVisitedProvinces.Commands.CreateUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Commands.DeleteUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Commands.UpdateUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Queries.GetAllUserVisitedProvinces;
using FlyLib.Application.UserVisitedProvinces.Queries.GetUserVisitedProvinceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserVisitedProvincesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserVisitedProvincesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todos los usuario visitado provincias.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserVisitedProvinceResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserVisitedProvincesQuery());
            return Ok(_mapper.Map<IEnumerable<UserVisitedProvinceResponseV1>>(result));
        }


        /// <summary>
        /// Obtener un usuarios visitado provincia por Id.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserVisitedProvinceResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserVisitedProvinceByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<UserVisitedProvinceResponseV1>(result));
        }

        /// <summary>
        /// Registrar un nuevo usuarios visitado provincia.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        [ProducesResponseType(typeof(UserVisitedProvinceResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateCountryRequestV1 request)
        {
            var cmd = _mapper.Map<CreateUserVisitedProvinceCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<UserVisitedProvinceResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        /// <summary>
        /// Actualizar un usuarios visitado provincia.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserVisitedProvinceRequestV1 request)
        {
            if (id != request.id) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdateUserVisitedProvinceCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar un usuarios visitado provincia.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteUserVisitedProvinceCommand(id));
            return NoContent();
        }
    }
}
