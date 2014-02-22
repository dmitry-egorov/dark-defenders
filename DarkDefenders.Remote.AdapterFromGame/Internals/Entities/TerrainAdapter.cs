using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class TerrainAdapter : ITerrainEvents
    {
        private readonly RemoteEventsPacker _packer;

        public TerrainAdapter(RemoteEventsPacker packer)
        {
            _packer = packer;
        }

        public void Created(string mapId)
        {
            _packer.MapLoaded(mapId);
        }

        public void Destroyed()
        {
            
        }
    }
}