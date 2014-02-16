using System;
using System.Collections.Generic;
using DarkDefenders.Remote.Model.Interface;
using DarkDefenders.Remote.Serialization.Internals;
using Infrastructure.Serialization;

namespace DarkDefenders.Remote.Serialization
{
    public class EventsSerializer
    {
        public int Serialize(byte[] buffer, IEnumerable<Action<IRemoteEvents>> events)
        {
            return (int)buffer.UsingBinaryWriter(writer =>
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