using System;

namespace Infrastructure.Network.Internals
{
    public static class ConnectionData
    {
        private static readonly Guid _connectGuid = new Guid("5930C42F-FA9C-4AFE-8B19-84A235B30AD6");
        private static readonly Guid _handshakeGuid = new Guid("D518FEF6-FE95-45C5-9411-C5B5F5F2C80D");
        private static readonly Guid _confirmGuid = new Guid("1F716F8C-3331-4370-B9B2-F6C0743049FC");

        public static byte[] GetConnectData()
        {
            return _connectGuid.ToByteArray();
        }

        public static bool VerifyConnectData(byte[] buffer)
        {
            return VerifyGuid(buffer, _connectGuid);
        }

        public static byte[] GetHandshakeData()
        {
            return _handshakeGuid.ToByteArray();
        }

        public static bool VerifyHandshakeData(byte[] buffer)
        {
            return VerifyGuid(buffer, _handshakeGuid);
        }

        public static byte[] GetConfirmData()
        {
            return _confirmGuid.ToByteArray();
        }

        public static bool VerifyConfirmData(byte[] data)
        {
            return VerifyGuid(data, _confirmGuid);
        }

        private static bool VerifyGuid(byte[] buffer, Guid expectedGuid)
        {
            if (buffer.Length != 16)
            {
                return false;
            }

            var guid = new Guid(buffer);

            return guid == expectedGuid;
        }
    }
}