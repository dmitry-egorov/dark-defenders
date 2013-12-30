using System;
using System.Drawing;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Map<T>
        where T: struct 
    {
        public Dimensions Dimensions
        {
            get { return _dimensions; }
        }

        public Map(Dimensions dimensions)
        {
            _dimensions = dimensions;
            _data = new T[dimensions.Width * dimensions.Height];
        }

        public T this[Point p]
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

        public T this[int x, int y]
        {
            get
            {
                if (IsNotWithinDimensions(x, y))
                {
                    return default(T);
                }

                return _data[y*_dimensions.Width + x];
            }
            set
            {
                if (IsNotWithinDimensions(x, y))
                {
                    throw new IndexOutOfRangeException();
                }

                _data[y*_dimensions.Width + x] = value;
            }
        }

        private bool IsNotWithinDimensions(int x, int y)
        {
            return x >= _dimensions.Width || x < 0 || y >= _dimensions.Height || y < 0;
        }

        public bool Equals(Map<T> other)
        {
            return _dimensions.Equals(other._dimensions) && _data.AllEquals(other._data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Map<T> && Equals((Map<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_dimensions.GetHashCode()*397) ^ _data.AllHashCode();
            }
        }

        private readonly Dimensions _dimensions;
        private readonly T[] _data;
    }
}