using MediatR;

namespace FlyLib.Application.Photos.Commands.DeletePhoto
{
    public class DeletePhotoCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeletePhotoCommand() { }

        public DeletePhotoCommand(int id)
        {
            Id = id;
        }
    }
}
