using System;
using System.IO;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static void UsingBinaryReader(this byte[] buffer, Action<BinaryReader> action)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                action(reader);
            }
        }

        public static long UsingBinaryWriter(this byte[] buffer, Action<BinaryWriter> action)
        {
            using (var stream = new MemoryStream(buffer))
            using (var writer = new BinaryWriter(stream))
            {
                action(writer);
                return stream.Position;
            }
        }

        public static IdentityOf<T> ReadIdentityOf<T>(this BinaryReader reader)
        {
            var value = reader.ReadInt64();
            return new IdentityOf<T>(value);
        }

        public static Vector ReadVector(this BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();

            return Vector.XY(x, y);
        }

        public static void Write<T>(this BinaryWriter writer, IdentityOf<T> identity)
        {
            writer.Write(identity.Value);
        }

        public static void Write(this BinaryWriter writer, Vector vector)
        {
            writer.Write((float)vector.X);
            writer.Write((float)vector.Y);
        }
    }
}