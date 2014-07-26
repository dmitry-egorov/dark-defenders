using System;
using System.IO;
using System.IO.Compression;

namespace Infrastructure.Serialization
{
    public static class SerializationExtensions
    {
        public static void UsingGZipBinaryReader(this byte[] buffer, Action<BinaryReader> action)
        {
            var array = buffer.Decompress();
            using (var readStream = new MemoryStream(array))
            using (var reader = new BinaryReader(readStream))
            {
                action(reader);
            }
        }

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
    }
}