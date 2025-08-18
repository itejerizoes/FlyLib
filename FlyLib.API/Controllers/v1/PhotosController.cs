using AutoMapper;
using FlyLib.API.DTOs.v1.Users.Requests;
using FlyLib.API.DTOs.v1.Users.Responses;
using FlyLib.Application.Common.Constants;
using FlyLib.Application.Users.Commands.CreateUser;
using FlyLib.Application.Users.Commands.DeleteUser;
using FlyLib.Application.Users.Commands.UpdateUser;
using FlyLib.Application.Users.Queries.GetAllUsers;
using FlyLib.Application.Users.Queries.GetUserById;
using FlyLib.Application.Photos.Commands.CreatePhoto;
using FlyLib.Application.Photos.Commands.DeletePhoto;
using FlyLib.Application.Photos.Commands.UpdatePhoto;
using FlyLib.Application.Photos.Queries.GetAllPhotos;
using FlyLib.Application.Photos.Queries.GetPhotoById;
using FlyLib.Infrastructure.Storages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlyLib.API.DTOs.v1.Photos.Requests;
using FlyLib.API.DTOs.v1.Photos.Responses;

namespace FlyLib.API.Controllers.v2
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly BlobStorageService _blobStorageService;


        public PhotosController(IMediator mediator, IMapper mapper, BlobStorageService blobStorageService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Obtener todos los visit photos.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
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
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
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
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost]
        [ProducesResponseType(typeof(PhotoResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreatePhotoRequestV1 request)
        {
            var cmd = _mapper.Map<CreatePhotoCommand>(request);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<PhotoResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        /// <summary>
        /// Actualizar una visit photo existente.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePhotoRequestV1 request)
        {
            if (id != request.Id) return BadRequest("Route id and body id must match");

            var cmd = _mapper.Map<UpdatePhotoCommand>(request);
            await _mediator.Send(cmd);
            return NoContent();
        }

        /// <summary>
        /// Eliminar una visit photo.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePhotoCommand(id));
            return NoContent();
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost("upload")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromServices] BlobStorageService blobService)
        {
            // Validación de formato y tamaño
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            if (file == null || !allowedTypes.Contains(file.ContentType))
                return BadRequest("Formato de imagen no permitido. Solo JPEG y PNG.");

            if (file.Length > 2 * 1024 * 1024) // 2MB
                return BadRequest("El tamaño máximo permitido es 2MB.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var url = await blobService.UploadAsync(file.OpenReadStream(), fileName, "Photos");

            // Aquí solo devuelves la URL, pero podrías guardar en BD usando tu flujo habitual
            return Created(url, new { url });
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost("upload-and-save")]
        [ProducesResponseType(typeof(PhotoResponseV1), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadAndSave([FromForm] IFormFile file, [FromForm] int VisitedId, [FromServices] BlobStorageService blobService)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png" };
            if (file == null || !allowedTypes.Contains(file.ContentType))
                return BadRequest("Formato de imagen no permitido. Solo JPEG y PNG.");

            if (file.Length > 2 * 1024 * 1024) // 2MB
                return BadRequest("El tamaño máximo permitido es 2MB.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var url = await blobService.UploadAsync(file.OpenReadStream(), fileName, "Photos");

            var createRequest = new CreatePhotoRequestV1(url, "Upload");

            var cmd = _mapper.Map<CreatePhotoCommand>(createRequest);
            var created = await _mediator.Send(cmd);

            var response = _mapper.Map<PhotoResponseV1>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        /// <summary>
        /// Genera una URL SAS para subir una imagen directamente a Azure Blob Storage.
        /// </summary>
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.User}")]
        [HttpPost("sign-upload")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUpload([FromBody] SignUploadRequestV1 request)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(request.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Formato de imagen no permitido. Solo JPG y PNG.");

            var userName = User.Identity?.Name ?? "unknown";
            var provinceName = string.IsNullOrWhiteSpace(request.ProvinceName) ? "provincia" : request.ProvinceName.Trim().Replace(" ", "_");

            // Prefijo para buscar blobs existentes
            var prefix = $"{userName}_{provinceName}_";
            int maxSeq = 0;

            var containerClient = _blobStorageService.GetContainerClient("Photos");
            await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
            {
                var name = blobItem.Name;
                var seqStart = prefix.Length;
                var seqEnd = name.IndexOf('.', seqStart);
                if (seqEnd > seqStart)
                {
                    var seqPart = name.Substring(seqStart, seqEnd - seqStart);
                    if (int.TryParse(seqPart, out int seq))
                    {
                        if (seq > maxSeq) maxSeq = seq;
                    }
                }
            }

            var nextSeq = maxSeq + 1;
            var generatedFileName = $"{userName}_{provinceName}_{nextSeq:D4}{ext}";

            var sasUrl = await _blobStorageService.GenerateUploadSasUrlAsync(
                generatedFileName, "Photos", TimeSpan.FromMinutes(10));

            return Ok(new { uploadUrl = sasUrl, fileName = generatedFileName });
        }
    }
}
