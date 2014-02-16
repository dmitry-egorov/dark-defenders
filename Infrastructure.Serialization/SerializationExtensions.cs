using System;
using System.IO;
using System.IO.Compression;
using Infrastructure.DDDES;
using Infrastructure.Math;

namespace Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static T UsingGZipBinaryReader<T>(this byte[] buffer, Func<BinaryReader, T> action)
        {
            var array = buffer.Decompress();
            using (var readStream = new MemoryStream(array))
            using (var reader = new BinaryReader(readStream))
            {
                return action(reader);
            }
        }

        public static byte[] Decompress(this byte[] gzip)
        {
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
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