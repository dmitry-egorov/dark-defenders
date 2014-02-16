using DarkDefenders.Domain.Model.Events;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteTerrain: ITerrainEvents
    {
        private readonly RemoteEventsPacker _packer;

        public RemoteTerrain(RemoteEventsPacker packer)
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