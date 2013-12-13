using DarkDefenders.Infrastructure.CommandHandling;

namespace DarkDefenders.Commands
{
    public class SpawnAvatarCommand: ICommand
    {
        public class Handler : ICommandHandler<SpawnAvatarCommand>
        {
            public void Handle(SpawnAvatarCommand command)
            {

            }
        }
    }
}
