using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DarkDefenders.Domain.Data.Infrastructure;
using Infrastructure.DDDES;
using Infrastructure.Serialization;
using Infrastructure.Util;

namespace DarkDefenders.ConsoleServer
{
    internal class EventsDataBroadcaster : IEventsListener<EventDataBase>
    {
        private readonly int _maximumEventsPerSend;
        private readonly Queue<EventDataBase> _queue = new Queue<EventDataBase>();
        private readonly ThreadLocal<byte[]> _buffers;
        private readonly UdpClient _client;
        private readonly IPEndPoint _ipEndPoint;

        public EventsDataBroadcaster(int maximumEventsPerSend)
        {
            _maximumEventsPerSend = maximumEventsPerSend;
            _buffers = new ThreadLocal<byte[]>(() => new byte[maximumEventsPerSend*100]);
            _client = new UdpClient();

            _ipEndPoint = new IPEndPoint(new IPAddress(new byte[] {127, 0, 0, 1}), 1337);
        }

        public void Recieve(EventDataBase entityEvent)
        {
            _queue.Enqueue(entityEvent);
        }

        public void Broadcast()
        {
            var events = _queue.DequeueAll().Take(_maximumEventsPerSend).ToList();

            Task.Run(() =>
            {
                var buffer = _buffers.Value;
                var bytes = (int) events.ProtoSerializeTo(buffer);
                _client.SendAsync(buffer, bytes, _ipEndPoint);
            });
        }
    }
}