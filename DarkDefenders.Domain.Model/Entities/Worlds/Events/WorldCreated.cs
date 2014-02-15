using DarkDefenders.Domain.Model.Events;
using Infrastructure.DDDES;

namespace DarkDefenders.Domain.Model.Entities.Worlds.Events
{
    internal class WorldCreated : Created<World>
    {
        private readonly string _mapId;

        public WorldCreated(World world, IStorage<World> storage, string mapId)
            : base(world, storage)
        {
            _mapId = mapId;
        }

        protected override void ApplyTo(World entity)
        {
            entity.Created(_mapId);
        }

        public override void Accept(IEventsReciever reciever)
        {
        }
    }
}