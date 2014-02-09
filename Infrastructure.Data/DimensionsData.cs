using System.Runtime.Serialization;
using Infrastructure.Math;
using Infrastructure.Util;
using ProtoBuf;

namespace Infrastructure.Data
{
    [ProtoContract]
    public class DimensionsData: SlowValueObject
    {
        [ProtoMember(1)]
        public int Width { get; set; }
        [ProtoMember(2)]
        public int Height { get; set; }

        public DimensionsData()//Protobuf
        {
        }

        public DimensionsData(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Dimensions ToDimensions()
        {
            return new Dimensions(Width, Height);
        }
    }
}