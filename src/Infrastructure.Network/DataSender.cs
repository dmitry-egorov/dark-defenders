using System;
using System.Net;
using System.Net.Sockets;
using Infrastructure.Network.Interfaces;
using Infrastructure.Udp;

namespace Infrastructure.Network.Internals
{
    public class DataSender : IDataSender
    {
        private readonly UdpClient _client;
        private readonly IPEndPoint _endpoint;

        public DataSender(UdpClient client, IPEndPoint endpoint)
        {
            _client = client;
            _endpoint = endpoint;
        }

        public void Send(byte[] data)
        {
            _client.SendAsync(data, _endpoint);
        }
    }
}