using DarkDefenders.Game.Model.Events;

namespace DarkDefenders.Remote.AdapterFromGame.Internals.Entities
{
    internal class TerrainAdapter : ITerrainEvents
    {
        private readonly RemoteState _adapter;

        public TerrainAdapter(RemoteState adapter)
        {
            _adapter = adapter;
        }

        public void Created(string mapId)
        {
            _adapter.MapLoaded(mapId);
        }

        public void Destroyed()
        {
            
        }
    }
}