using System.IO;
using Infrastructure.Math;

namespace Infrastructure.Serialization.Math
{
    public static class SerializationExtensions
    {
        public static Vector ReadVector(this BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();

            return Vector.XY(x, y);
        }

        public static void Write(this BinaryWriter writer, Vector vector)
        {
            writer.Write((float)vector.X);
            writer.Write((float)vector.Y);
        }
    }
}
