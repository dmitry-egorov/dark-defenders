using System;
using System.Collections.Generic;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization.Internals;
using Infrastructure.Serialization;

namespace DarkDefenders.Remote.Serialization
{
    public class EventsSerializer
    {
        public byte[] Serialize(IEnumerable<Action<IRemoteEvents>> events)
        {
            return Using.GZipBinaryWriter(writer =>
            {
                var reciever = new SerializingReciever(writer);

                foreach (var e in events)
                {
                    e(reciever);
                }
            });
        }
    }
}