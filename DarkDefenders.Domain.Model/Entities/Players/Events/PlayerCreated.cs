using DarkDefenders.Domain.Model.Entities.Creatures;
using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Players.Events
{
    internal class PlayerCreated : Created<Player>
    {
        private readonly Creature _creature;

        public PlayerCreated(Player player, IStorage<Player> storage, Creature creature) : base(player, storage)
        {
            _creature = creature;
        }

        protected override void ApplyTo(Player entity)
        {
            entity.Created(_creature);
        }

        public override void Accept(IEventsReciever reciever)
        {
            reciever.PlayerCreated(_creature.Id);
        }
    }
}