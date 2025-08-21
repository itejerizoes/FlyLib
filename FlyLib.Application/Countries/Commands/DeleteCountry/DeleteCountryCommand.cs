using MediatR;

namespace FlyLib.Application.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteCountryCommand() { }

        public DeleteCountryCommand(int id)
        {
            Id = id;
        }
    }
}
