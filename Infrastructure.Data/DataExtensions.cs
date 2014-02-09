using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Math;
using Infrastructure.Physics;

namespace Infrastructure.Data
{
    public static class DataExtensions
    {
        public static MomentumData ToData(this Momentum momentum)
        {
            return new MomentumData((float) momentum.Value.X, (float) momentum.Value.Y);
        }

        public static VectorData ToData(this Vector vector)
        {
            return new VectorData((float) vector.X, (float) vector.Y);
        }

        public static IEnumerable<Vector> ToVectors(this IEnumerable<VectorData> datas)
        {
            return datas.Select(x => x.ToVector());
        }

        public static IEnumerable<VectorData> ToData(this IEnumerable<Vector> vectors)
        {
            return vectors.Select(x => x.ToData());
        }
    }
}