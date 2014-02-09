using DarkDefenders.Domain.Entities.Creatures;
using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Adapters
{
    internal class WorldAdapter : IWorld
    {
        private readonly IEventsProcessor _processor;
        private readonly EntityAdapter<World> _world;

        public WorldAdapter(IEventsProcessor processor, EntityAdapter<World> worldAdapter)
        {
            _processor = processor;
            _world = worldAdapter;
        }

        public IPlayer AddPlayer()
        {
            var creatureAdapter = _world.Commit(x => x.SpawnPlayerAvatar());

            return new PlayerAdapter(creatureAdapter);
        }

        public void SpawnHero()
        {
            _world.Commit(x => x.SpawnHero());
        }

        public void ChangeSpawnHeroes(bool enabled)
        {
            _world.Commit(x => x.ChangeSpawnHeroes(enabled));
        }
    }
}