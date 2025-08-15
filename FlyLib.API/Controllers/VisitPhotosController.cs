using FlyLib.Application.Common.Constants;
using FlyLib.Application.VisitPhotos.Commands.CreateVisitPhoto;
using FlyLib.Application.VisitPhotos.Commands.DeleteVisitPhoto;
using FlyLib.Application.VisitPhotos.Commands.UpdateVisitPhoto;
using FlyLib.Application.VisitPhotos.Queries.GetAllVisitPhotos;
using FlyLib.Application.VisitPhotos.Queries.GetVisitPhotoById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitPhotosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VisitPhotosController(IMediator mediator) => _mediator = mediator;

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetAllVisitPhotosQuery()));

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetVisitPhotoByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVisitPhotoCommand cmd)
        {
            var created = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateVisitPhotoCommand cmd)
        {
            if (id != cmd.Id) return BadRequest("Route id and body id must match");
            await _mediator.Send(cmd);
            return NoContent();
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteVisitPhotoCommand(id));
            return NoContent();
        }
    }
}
