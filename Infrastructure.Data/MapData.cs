using System.Runtime.Serialization;
using Infrastructure.Math;
using Infrastructure.Util;
using ProtoBuf;

namespace Infrastructure.Data
{
    [ProtoContract]
    public class MapData<T> : SlowValueObject
        where T : struct
    {
        [ProtoMember(1)]
        public T[] Values { get; private set; }
        [ProtoMember(2)]
        public DimensionsData Dimensions { get; private set; }
        [ProtoMember(3)]
        public T DefaultItem { get; private set; }

        public MapData()//Protobuf
        {
        }

        public MapData(T[] values, DimensionsData dimensions, T defaultItem)
        {
            Values = values;
            Dimensions = dimensions;
            DefaultItem = defaultItem;
        }

        public Map<T> ToMap()
        {
            var dimensions = Dimensions.ToDimensions();
            var map = new Map<T>(dimensions, DefaultItem);

            var k = 0;
            for (var y = 0; y < dimensions.Height; y++)
            {
                for (var x = 0; x < dimensions.Width; x++)
                {
                    map[x, y] = Values[k];
                    k++;
                }
            }

            return map;
        }
    }
}