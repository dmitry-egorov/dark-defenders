using System.IO;
using Infrastructure.DDDES;

namespace Infrastructure.Serialization.DDDES
{
    public static class SerializationExtensions
    {
        public static IdentityOf<T> ReadIdentityOf<T>(this BinaryReader reader)
        {
            var value = reader.ReadInt64();
            return new IdentityOf<T>(value);
        }

        public static void Write<T>(this BinaryWriter writer, IdentityOf<T> identity)
        {
            writer.Write(identity.Value);
        }
    }
}
