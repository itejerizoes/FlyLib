using AutoMapper;
using FlyLib.API.DTOs.v1.Countries.Requests;
using FlyLib.API.DTOs.v1.Countries.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Countries.Commands.CreateCountry;
using FlyLib.Application.Countries.Commands.DeleteCountry;
using FlyLib.Application.Countries.Commands.UpdateCountry;
using FlyLib.Application.Countries.Queries.GetAllCountries;
using FlyLib.Application.Countries.Queries.GetCountryById;
using FlyLib.Application.Countries.Queries.GetCountryByName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CountriesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        /// <summary>
        /// Obtener todos los países.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CountryResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(_mapper.Map<IEnumerable<CountryResponseV1>>(result));
        }

        /// <summary>
        /// Obtener un país por Id.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CountryResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCountryByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<CountryResponseV1>(result));
        }

        /// <summary>
        /// Obtener un país por nombre.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("byName/{name}")]
        [ProducesResponseType(typeof(CountryResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _mediator.Send(new GetCountryByNameQuery(name));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<CountryResponseV1>(result));
        }

        /// <summary>
        /// Registrar un nuevo país.
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CountryResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateCountryRequestV1 request)
        {
            var cmd = _mapper.Map<CreateCountryCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<CountryResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.CountryId }, response);
        }

        /// <summary>
        /// Actualizar un país.
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCountryRequestV1 request)
        {
            if (id != request.CountryId) return BadRequest("Route id and body id must match");

            try
            {
                var cmd = _mapper.Map<UpdateCountryCommand>(request);
                await _mediator.Send(cmd);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Eliminar un país.
        /// </summary>
        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCountryCommand(id));
            return NoContent();
        }
    }
}
