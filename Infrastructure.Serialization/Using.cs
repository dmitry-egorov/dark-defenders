using System;
using System.IO;
using System.IO.Compression;

namespace Infrastructure.Serialization
{
    public static class Using
    {
        public static byte[] GZipBinaryWriter(Action<BinaryWriter> action)
        {
            using (var stream = new MemoryStream())
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                using (var writer = new BinaryWriter(gzip))
                {
                    action(writer);
                }

                return stream.ToArray();
            }
        }
    }
}