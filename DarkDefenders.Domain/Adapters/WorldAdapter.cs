using DarkDefenders.Domain.Entities.Worlds;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES.Implementations.Domain;

namespace DarkDefenders.Domain.Adapters
{
    internal class WorldAdapter : IWorld
    {
        private readonly EntityAdapter<World> _world;

        public WorldAdapter(EntityAdapter<World> worldAdapter)
        {
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