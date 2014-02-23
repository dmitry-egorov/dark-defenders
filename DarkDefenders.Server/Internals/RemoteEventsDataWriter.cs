using System;
using System.Collections.Generic;
using System.IO;
using DarkDefenders.Remote.AdapterFromGame;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization;
using Infrastructure.Network.Subscription.Server.Interfaces;

namespace DarkDefenders.Server.Internals
{
    public class RemoteEventsDataWriter : IEventsDataWriter
    {
        private readonly IRemoteEventsSource _adapter;

        public RemoteEventsDataWriter(IRemoteEventsSource adapter)
        {
            _adapter = adapter;
        }

        public void WriteEventsData(BinaryWriter writer)
        {
            var events = _adapter.GetEvents();

            Serialize(writer, events);
        }

        public void WriteInitialEventsData(BinaryWriter writer)
        {
            var events = _adapter.GetCurrentStateEvents();

            Serialize(writer, events);
        }

        private static void Serialize(BinaryWriter writer, IEnumerable<Action<IRemoteEvents>> events)
        {
            var serializer = new RemoteEventsSerializer(writer);
            foreach (var e in events)
            {
                e(serializer);
            }
        }
    }
}