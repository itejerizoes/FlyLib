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

namespace FlyLib.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvincesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProvincesController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetAllProvincesQuery()));

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProvinceByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("byName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _mediator.Send(new GetProvinceByNameQuery(name));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProvinceCommand cmd)
        {
            var created = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = created.ProvinceId }, created);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateProvinceCommand cmd)
        {
            if (id != cmd.ProvinceId) return BadRequest("Route id and body id must match");
            await _mediator.Send(cmd);
            return NoContent();
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProvinceCommand(id));
            return NoContent();
        }
    }
}
