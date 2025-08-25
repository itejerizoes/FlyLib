using AutoMapper;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Photos.Commands.CreatePhoto;
using FlyLib.Application.Photos.Commands.DeletePhoto;
using FlyLib.Application.Photos.Commands.UpdatePhoto;
using FlyLib.Application.Photos.Queries.GetAllPhotos;
using FlyLib.Application.Photos.Queries.GetPhotoById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Photos.Responses;

namespace FlyLib.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;


        public PhotosController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todos los visit photos.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PhotoResponseV1>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPhotosQuery());
            return Ok(_mapper.Map<IEnumerable<PhotoResponseV1>>(result));
        }

        /// <summary>
        /// Obtener un visit photo por su Id.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PhotoResponseV1), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPhotoByIdQuery(id));
            if (result is null) return NotFound();

            return Ok(_mapper.Map<PhotoResponseV1>(result));
        }

        /// <summary>
        /// Crear un nuevo visit photo.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        [ProducesResponseType(typeof(PhotoResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreatePhotoRequestV1 request)
        {
            var cmd = _mapper.Map<CreatePhotoCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<PhotoResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Actualizar una visit photo existente.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePhotoRequestV1 request)
        {
            if (id != request.PhotoId) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdatePhotoCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar una visit photo.
        /// </summary>
        [Authorize(AuthenticationSchemes = "Bearer", Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePhotoCommand(id));
            return NoContent();
        }
    }
}
