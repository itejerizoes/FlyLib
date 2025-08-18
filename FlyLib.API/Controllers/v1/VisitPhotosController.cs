using AutoMapper;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using FlyLib.API.DTOs.v1.VisitPhoto.Requests;
using FlyLib.API.DTOs.v1.VisitPhoto.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.DeleteUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.Users.Queries.GetAllUsers;
using FlyLib.Application.Users.Queries.GetUserById;
using FlyLib.Application.VisitPhotos.Commands.CreateVisitPhoto;
using FlyLib.Application.VisitPhotos.Commands.DeleteVisitPhoto;
using FlyLib.Application.VisitPhotos.Commands.UpdateVisitPhoto;
using FlyLib.Application.VisitPhotos.Queries.GetAllVisitPhotos;
using FlyLib.Application.VisitPhotos.Queries.GetVisitPhotoById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyLib.API.Controllers.v2
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VisitPhotosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public VisitPhotosController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todos los visit photos.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VisitPhotoResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllVisitPhotosQuery());
            return Ok(_mapper.Map<IEnumerable<VisitPhotoResponseV1>>(result));
        }

        /// <summary>
        /// Obtener un visit photo por su Id.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VisitPhotoResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetVisitPhotoByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<VisitPhotoResponseV1>(result));
        }

        /// <summary>
        /// Crear un nuevo visit photo.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        [ProducesResponseType(typeof(VisitPhotoResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateVisitPhotoRequestV1 request)
        {
            var cmd = _mapper.Map<CreateVisitPhotoCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<VisitPhotoResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        /// <summary>
        /// Actualizar una visit photo existente.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVisitPhotoRequestV1 request)
        {
            if (id != request.Id) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdateVisitPhotoCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar una visit photo.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:string}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteVisitPhotoCommand(id));
            return NoContent();
        }
    }
}
