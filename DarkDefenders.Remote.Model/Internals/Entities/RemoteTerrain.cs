using DarkDefenders.Domain.Model.Events;
using DarkDefenders.Remote.Model.Interface;

namespace DarkDefenders.Remote.Model.Internals.Entities
{
    internal class RemoteTerrain: ITerrainEvents
    {
        private readonly IRemoteEvents _reciever;

        public RemoteTerrain(IRemoteEvents reciever)
        {
            _reciever = reciever;
        }

        public void Created(string mapId)
        {
            _reciever.MapLoaded(mapId);
        }

        public void Destroyed()
        {
            
        }
    }
}