using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DarkDefenders.Domain.Model;
using DarkDefenders.Domain.Serialization;
using Infrastructure.DDDES;

namespace DarkDefenders.ConsoleServer
{
    internal class EventsDataBroadcaster : IEventsListener<IEventsReciever>
    {
        private readonly ThreadLocal<byte[]> _buffers;
        private readonly UdpClient _client;
        private readonly IPEndPoint _ipEndPoint;
        private readonly EventsSerializer _serializer;

        public EventsDataBroadcaster()
        {
            _buffers = new ThreadLocal<byte[]>(() => new byte[1000000]);
            _client = new UdpClient();

            _ipEndPoint = new IPEndPoint(new IPAddress(new byte[] {127, 0, 0, 1}), 1337);

            _serializer = new EventsSerializer();
        }

        public void Recieve(IEnumerable<IAcceptorOf<IEventsReciever>> events)
        {
            Task.Run(() =>
            {
                var buffer = _buffers.Value;

                var bytes = _serializer.Serialize(buffer, events);

                if (bytes == 0)
                {
                    return;
                }

                _client.SendAsync(buffer, bytes, _ipEndPoint);
            });
        }
    }
}