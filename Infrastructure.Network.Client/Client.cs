using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Network.Interfaces;
using Infrastructure.Network.Internals;
using Infrastructure.Udp;

namespace Infrastructure.Network.Client
{
    public class Client
    {
        private readonly IPEndPoint _endPoint;
        private readonly IConnectionsManager _connectionsManager;

        public static Client Create(IPAddress address, int port, IConnectionsManager connectionsManager)
        {
            var endPoint = new IPEndPoint(address, port);

            return new Client(endPoint, connectionsManager);
        }

        private Client(IPEndPoint endPoint, IConnectionsManager connectionsManager)
        {
            _endPoint = endPoint;
            _connectionsManager = connectionsManager;
        }

        public async void RunAsync(CancellationToken token)
        {
            var client = new UdpClient();

            token.Register(client.Close);

            try
            {
                Console.Write("Connecting...");

                var connected = await TryConnectAsync(client);
                if (!connected)
                {
                    Console.WriteLine("error. Wrong handshake data!");
                    return;
                }

                Console.WriteLine("done.");

                await RunRecieveAsync(token, client);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private async Task<bool> TryConnectAsync(UdpClient client)
        {
            var connectionData = ConnectionData.GetConnectData();

            await client.SendAsync(connectionData, _endPoint);

            var handshake = await client.ReceiveAsync();

            if (!ConnectionData.VerifyHandshakeData(handshake.Buffer))
            {
                return false;
            }

            var confirmData = ConnectionData.GetConfirmData();

            await client.SendAsync(confirmData, _endPoint);

            return true;
        }

        private async Task RunRecieveAsync(CancellationToken token, UdpClient client)
        {
            var remoteReciever = new DataSender(client, _endPoint);

            var reciever = _connectionsManager.OpenConnection(remoteReciever);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var broadcastData = await client.ReceiveAsync();

                    reciever.Recieve(broadcastData.Buffer);
                }
            }
            catch (SocketException)
            {
                _connectionsManager.CloseConnection(remoteReciever);
            }
        }
    }
}