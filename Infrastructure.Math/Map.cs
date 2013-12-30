using System.Drawing;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Map
    {
        public Dimensions Dimensions
        {
            get { return _dimensions; }
        }

        public Map(Dimensions dimensions)
        {
            _dimensions = dimensions;
            _data = new byte[dimensions.Width * dimensions.Height];
        }

        public byte this[Point p]
        {
            get
            {
                return this[p.X, p.Y];
            }
            set
            {
                this[p.X, p.Y] = value;
            }
        }

        public byte this[int x, int y]
        {
            get
            {
                return _data[y*_dimensions.Width + x];
            }
            set
            {
                _data[y*_dimensions.Width + x] = value;
            }
        }

        public bool Equals(Map other)
        {
            return _dimensions.Equals(other._dimensions) && _data.AllEquals(other._data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Map && Equals((Map) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_dimensions.GetHashCode()*397) ^ _data.AllHashCode();
            }
        }

        private readonly Dimensions _dimensions;
        private readonly byte[] _data;
    }
}