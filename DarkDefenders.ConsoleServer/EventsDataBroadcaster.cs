using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using DarkDefenders.Remote.Model;
using DarkDefenders.Remote.Serialization;
using Infrastructure.DDDES;

namespace DarkDefenders.ConsoleServer
{
    internal class EventsDataBroadcaster : IEventsListener<IRemoteEvents>
    {
        private readonly UdpClient _client;
        private readonly IPEndPoint _ipEndPoint;
        private readonly EventsSerializer _serializer;

        public EventsDataBroadcaster()
        {
            _client = new UdpClient();

            _ipEndPoint = new IPEndPoint(new IPAddress(new Byte[] {127, 0, 0, 1}), 1337);

            _serializer = new EventsSerializer();
        }

        public void Recieve(IEnumerable<Action<IRemoteEvents>> events)
        {
            SendAsync(events);
        }

        private async void SendAsync(IEnumerable<Action<IRemoteEvents>> events)
        {
            await Task.Run(() =>
            {
                var buffer = _serializer.Serialize(events);
                var bytes = buffer.Length;

                if (bytes == 0)
                {
                    return;
                }

                _client.SendAsync(buffer, bytes, _ipEndPoint);
            });
        }
    }
}