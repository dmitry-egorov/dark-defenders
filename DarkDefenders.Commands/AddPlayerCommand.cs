using DarkDefenders.Infrastructure.CommandHandling;

namespace DarkDefenders.Commands
{
    public class AddPlayerCommand : ICommand
    {
        public int RequestId { get; private set; }

        public AddPlayerCommand(int requestId)
        {
            RequestId = requestId;
        }

        public class Handler: ICommandHandler<AddPlayerCommand>
        {
            public void Handle(AddPlayerCommand command)
            {

            } 
        }
    }
}