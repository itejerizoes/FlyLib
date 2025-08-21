using MediatR;

namespace FlyLib.Application.Provinces.Commands.DeleteProvince
{
    public class DeleteProvinceCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteProvinceCommand() { }

        public DeleteProvinceCommand(int id)
        {
            Id = id;
        }
    }
}
