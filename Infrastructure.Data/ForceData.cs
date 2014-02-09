using Infrastructure.Util;
using ProtoBuf;

namespace Infrastructure.Data
{
    [ProtoContract]
    public class ForceData : SlowValueObject
    {
        [ProtoMember(1)]
        public float X { get; private set; }
        [ProtoMember(2)]
        public float Y { get; private set; }

        public ForceData()//Protobuf
        {
        }

        public ForceData(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}