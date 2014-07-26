using System;
using System.Collections.Generic;
using System.IO;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization.Internals;
using Infrastructure.DDDES;
using Infrastructure.Math;
using Infrastructure.Network.Subscription.Client.Interfaces;
using Infrastructure.Serialization.DDDES;
using Infrastructure.Serialization.Math;
using Infrastructure.Util;

namespace DarkDefenders.Remote.Serialization
{
    public class EventsDeserializer : IEventsDataInterpreter
    {
        private readonly IEventsListener<IRemoteEvents> _reciever;

        public EventsDeserializer(IEventsListener<IRemoteEvents> reciever)
        {
            _reciever = reciever;
        }

        public Action Interpret(BinaryReader reader)
        {
            var actions = DeserializeAll(reader).AsReadOnly();

            return () =>
            {
                foreach (var action in actions)
                {
                    action();
                }
            };
        }

        private IEnumerable<Action> DeserializeAll(BinaryReader reader)
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
                    case SerializableEvents.ChangedDirection:
                        yield return ReadChangedDirection(reader);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Action ReadMapLoaded(BinaryReader reader)
        {
            var mapId = reader.ReadString();
            
            return Yield(r => r.MapLoaded(mapId));
        }

        private Action ReadRigidBodyCreated(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteEntity>();
            var position = reader.ReadVector();
            var type = (RemoteEntityType)reader.ReadByte();

            return Yield(r => r.Created(id, position, type));
        }

        private Action ReadRigidBodyDestroyed(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteEntity>();
            return Yield(r => r.Destroyed(id));
        }

        private Action ReadMoved(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteEntity>();
            var newPosition = reader.ReadVector();

            return Yield(r => r.Moved(id, newPosition));
        }

        private Action ReadChangedDirection(BinaryReader reader)
        {
            var id = reader.ReadIdentityOf<RemoteEntity>();
            var newPosition = reader.ReadByteEnum<Direction>();

            return Yield(r => r.ChangedDirection(id, newPosition));
        }

        private Action Yield(Action<IRemoteEvents> action)
        {
            return () => _reciever.Recieve(As.Enumerable(action));
        }
    }
}