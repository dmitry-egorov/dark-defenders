using System.IO;
using DarkDefenders.Remote.Model;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Serialization;

namespace DarkDefenders.Remote.Serialization.Internals
{
    internal class SerializingReciever : IRemoteEvents
    {
        private readonly BinaryWriter _writer;

        public SerializingReciever(BinaryWriter writer)
        {
            _writer = writer;
        }

        public void MapLoaded(string mapId)
        {
            _writer.Write((short) SerializableEvents.MapLoaded);
            _writer.Write(mapId);
        }

        public void Created(IdentityOf<RemoteRigidBody> id, Vector position, RemoteEntityType type)
        {
            _writer.Write((short)SerializableEvents.Created);
            _writer.Write(id);
            _writer.Write(position);
            _writer.Write((byte)type);
        }

        public void Destroyed(IdentityOf<RemoteRigidBody> id)
        {
            _writer.Write((short)SerializableEvents.Destroyed);
            _writer.Write(id);
        }

        public void Moved(IdentityOf<RemoteRigidBody> id, Vector newPosition)
        {
            _writer.Write((short)SerializableEvents.Moved);
            _writer.Write(id);
            _writer.Write(newPosition);
        }
    }
}