using System.IO;

namespace Infrastructure.Serialization
{
    public static class Proto
    {
        public static long ProtoSerializeTo<T>(this T instance, byte[] buffer)
        {
            return Serialize(instance, buffer);
        }

        public static T ProtoDeserializeAs<T>(this byte[] buffer)
        {
            return Deserialize<T>(buffer);
        }

        public static T Deserialize<T>(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }
        public static long Serialize<T>(T instance, byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                ProtoBuf.Serializer.Serialize(stream, instance);
                return stream.Position;
            }
        }
    }
}
