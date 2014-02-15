using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.PlayerSpawners.Events
{
    internal class PlayerSpawnerCreated : Created<PlayerSpawner>
    {
        private readonly string _mapId;

        public PlayerSpawnerCreated(PlayerSpawner entity, IStorage<PlayerSpawner> storage, string mapId) : base(entity, storage)
        {
            _mapId = mapId;
        }

        protected override void ApplyTo(PlayerSpawner entity)
        {
            entity.Created(_mapId);
        }

        public override void Accept(IEventsReciever reciever)
        {
        }
    }
}