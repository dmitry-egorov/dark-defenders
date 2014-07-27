using System.IO;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization.Internals;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Serialization.DDDES;
using Infrastructure.Serialization.Math;

namespace DarkDefenders.Remote.Serialization
{
    public class RemoteEventsSerializer : IRemoteEvents
    {
        private readonly BinaryWriter _writer;

        public RemoteEventsSerializer(BinaryWriter writer)
        {
            _writer = writer;
        }

        public void MapLoaded(string mapId)
        {
            _writer.Write((short)SerializableEvents.MapLoaded);
            _writer.Write(mapId);
        }

        public void Created(IdentityOf<RemoteEntity> id, Vector position, RemoteEntityType type)
        {
            _writer.Write((short)SerializableEvents.Created);
            _writer.Write(id);
            _writer.Write(position);
            _writer.Write((byte)type);
        }

        public void ChangedDirection(IdentityOf<RemoteEntity> id, HorizontalDirection newDirection)
        {
            _writer.Write((short)SerializableEvents.ChangedDirection);
            _writer.Write(id);
            _writer.WriteByteEnum(newDirection);
        }

        public void Destroyed(IdentityOf<RemoteEntity> id)
        {
            _writer.Write((short)SerializableEvents.Destroyed);
            _writer.Write(id);
        }

        public void Moved(IdentityOf<RemoteEntity> id, Vector newPosition)
        {
            _writer.Write((short)SerializableEvents.Moved);
            _writer.Write(id);
            _writer.Write(newPosition);
        }
    }

}