using DarkDefenders.Domain.Player.Event;

namespace DarkDefenders.Domain.Player
{
    public class RootState : IEventReciever
    {
        public Id RootId { get; private set; }

        public void Apply(Created created)
        {
            RootId = created.RootId;
        }
    }
}