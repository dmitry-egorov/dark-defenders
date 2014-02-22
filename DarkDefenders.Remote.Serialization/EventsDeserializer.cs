using System;
using System.Collections.Generic;
using System.IO;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization.Internals;
using Infrastructure.Serialization;
using Infrastructure.Util;

namespace DarkDefenders.Remote.Serialization
{
    public class EventsDeserializer
    {
        public IEnumerable<Action<IRemoteEvents>> Deserialize(byte[] eventData)
        {
            return eventData
            .UsingGZipBinaryReader
            (reader => 
                Read(reader)
                .AsReadOnly()
            );
        }

        private static IEnumerable<Action<IRemoteEvents>> Read(BinaryReader reader)
        {
            while (reader.PeekChar() != -1)
            {
                var eventType = (SerializableEvents) reader.ReadInt16();

                switch (eventType)
                {
                    case SerializableEvents.MapLoaded:
                        yield return ReadMapLoaded(reader);
                        break;
                    case SerializableEvents.Created:
                        yield return ReadRigidBodyCreated(reader);
                        break;
                    case SerializableEvents.Destroyed:
                        yield return ReadRigidBodyDestroyed(reader);
                        break;
                    case SerializableEvents.Moved:
                        yield return ReadMoved(reader);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static Action<IRemoteEvents> ReadMapLoaded(BinaryReader reader)
        {
            var mapId = reader.ReadString();
            return r => r.MapLoaded(mapId);
        }

        private static Action<IRemoteEvents> ReadRigidBodyCreated(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteRigidBody>();
            var position = reader.ReadVector();
            var type = (RemoteEntityType)reader.ReadByte();

            return r => r.Created(id, position, type);
        }

        private static Action<IRemoteEvents> ReadRigidBodyDestroyed(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteRigidBody>();
            return r => r.Destroyed(id);
        }

        private static Action<IRemoteEvents> ReadMoved(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteRigidBody>();
            var newPosition = reader.ReadVector();

            return r => r.Moved(id, newPosition);
        }
    }
}