using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Infrastructure.Udp
{
    public static class UdpClientExtensions
    {
        public static Task<int> SendAsync(this UdpClient client, byte[] data, IPEndPoint endPoint)
        {
            return client.SendAsync(data, data.Length, endPoint);
        }
    }
}
