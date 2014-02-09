using Infrastructure.Math;
using Infrastructure.Util;
using ProtoBuf;

namespace Infrastructure.Data
{
    [ProtoContract]
    public class VectorData : SlowValueObject
    {
        [ProtoMember(1)]
        public float X { get; private set; }
        [ProtoMember(2)]
        public float Y { get; private set; }

        public VectorData()//Protobuf
        {
        }

        public VectorData(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector ToVector()
        {
            return new Vector(X, Y);
        }
    }
}