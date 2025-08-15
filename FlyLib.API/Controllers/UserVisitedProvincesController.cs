using FlyLib.Application.Common.Constants;
using FlyLib.Application.UserVisitedProvinces.Commands.CreateUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Commands.DeleteUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Commands.UpdateUserVisitedProvince;
using FlyLib.Application.UserVisitedProvinces.Queries.GetAllUserVisitedProvinces;
using FlyLib.Application.UserVisitedProvinces.Queries.GetUserVisitedProvinceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserVisitedProvincesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserVisitedProvincesController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetAllUserVisitedProvincesQuery()));

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserVisitedProvinceByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVisitedProvinceCommand cmd)
        {
            var created = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateUserVisitedProvinceCommand cmd)
        {
            if (id != cmd.Id) return BadRequest("Route id and body id must match");
            await _mediator.Send(cmd);
            return NoContent();
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteUserVisitedProvinceCommand(id));
            return NoContent();
        }
    }
}
