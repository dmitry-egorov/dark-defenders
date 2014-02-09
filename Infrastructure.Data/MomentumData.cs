using System.Runtime.Serialization;
using Infrastructure.Math;
using Infrastructure.Physics;
using Infrastructure.Util;
using ProtoBuf;

namespace Infrastructure.Data
{
    [ProtoContract]
    public class MomentumData : SlowValueObject
    {
        [ProtoMember(1)]
        public float X { get; private set; }
        [ProtoMember(2)]
        public float Y { get; private set; }

        public MomentumData()//Protobuf
        {
        }

        public MomentumData(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Momentum ToMomentum()
        {
            return new Momentum(new Vector(X, Y));
        }
    }
}