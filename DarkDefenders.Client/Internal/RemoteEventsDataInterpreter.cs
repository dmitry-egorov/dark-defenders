using System;
using System.IO;
using DarkDefenders.Remote.Serialization;
using Infrastructure.Network.Subscription.Client.Interfaces;

namespace DarkDefenders.Client.Internal
{
    internal class RemoteEventsDataInterpreter : IEventsDataInterpreter
    {
        private readonly EventsDeserializer _deserializer;

        public RemoteEventsDataInterpreter(EventsDeserializer deserializer)
        {
            _deserializer = deserializer;
        }

        public Action InterpretInitial(BinaryReader reader)
        {
            return _deserializer.Deserialize(reader);
        }

        public Action Interpret(BinaryReader reader)
        {
            return _deserializer.Deserialize(reader);
        }
    }
}