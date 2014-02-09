using System.Collections.Generic;
using DarkDefenders.Domain.Interfaces;
using DarkDefenders.Domain.Serialization.Internals;
using Infrastructure.DDDES;
using Infrastructure.Serialization;

namespace DarkDefenders.Domain.Serialization
{
    public class EventsSerializer
    {
        public int Serialize(byte[] buffer, IEnumerable<IAcceptorOf<IEventsReciever>> events)
        {
            return (int)buffer.UsingBinaryWriter(writer =>
            {
                var reciever = new SerializingReciever(writer);

                foreach (var e in events)
                {
                    e.Accept(reciever);
                }
            });
        }
    }
}