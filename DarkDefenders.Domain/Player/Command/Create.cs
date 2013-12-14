using Infrastructure.DDDEventSourcing.Implementations.Domain;

namespace DarkDefenders.Domain.Player.Command
{
    public class Create : CommandBase<Id>
    {
        public Create(Id playerId) : base(playerId)
        {
        }
    }
}