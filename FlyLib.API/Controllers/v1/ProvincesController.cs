using AutoMapper;
using FlyLib.API.DTOs.v1.Provinces.Requests;
using FlyLib.API.DTOs.v1.Provinces.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Provinces.Commands.CreateProvince;
using FlyLib.Application.Provinces.Commands.DeleteProvince;
using FlyLib.Application.Provinces.Commands.UpdateProvince;
using FlyLib.Application.Provinces.Queries.GetAllProvinces;
using FlyLib.Application.Provinces.Queries.GetProvinceById;
using FlyLib.Application.Provinces.Queries.GetProvinceByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProvincesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProvincesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas las provincias.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProvinceResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProvincesQuery());
            return Ok(_mapper.Map<IEnumerable<ProvinceResponseV1>>(result));
        }

        /// <summary>
        /// Obtener una provincia por su Id.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProvinceResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProvinceByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<ProvinceResponseV1>(result));
        }

        /// <summary>
        /// Obtener una provincia por su nombre.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("byName/{name}")]
        [ProducesResponseType(typeof(ProvinceResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _mediator.Send(new GetProvinceByNameQuery(name));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<ProvinceResponseV1>(result));
        }

        /// <summary>
        /// Crear una nueva provincia.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = AppRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(ProvinceResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateProvinceRequestV1 request)
        {
            var cmd = _mapper.Map<CreateProvinceCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<ProvinceResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.ProvinceId }, response);
        }

        /// <summary>
        /// Actualizar una provincia existente.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = AppRoles.Admin)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProvinceRequestV1 request)
        {
            if (id != request.ProvinceId) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdateProvinceCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar una provincia.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = AppRoles.Admin)]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProvinceCommand(id));
            return NoContent();
        }
    }
}
