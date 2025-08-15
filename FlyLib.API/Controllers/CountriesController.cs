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

namespace FlyLib.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CountriesController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetAllCountriesQuery()));

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCountryByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("byName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _mediator.Send(new GetCountryByNameQuery(name));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCountryCommand cmd)
        {
            var created = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = created.CountryId }, created);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCountryCommand cmd)
        {
            if (id != cmd.CountryId) return BadRequest("Route id and body id must match");
            await _mediator.Send(cmd);
            return NoContent();
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCountryCommand(id));
            return NoContent();
        }
    }
}
