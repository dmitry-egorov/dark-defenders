using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Map<T>: IEnumerable<T>
        where T: struct
    {
        private readonly Dimensions _dimensions;
        private readonly T[] _data;
        private readonly T _defaultItem;

        public Dimensions Dimensions
        {
            get { return _dimensions; }
        }

        public Map(Dimensions dimensions, T defaultItem)
        {
            _dimensions = dimensions;
            _data = new T[dimensions.Width * dimensions.Height];
            _defaultItem = defaultItem;
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
                    return _defaultItem;
                }

                return _data[y*_dimensions.Width + x];
            }
            set
            {
                if (IsNotWithinDimensions(x, y))
                {
                    throw new IndexOutOfRangeException();
                }

                _data[y * _dimensions.Width + x] = value;
            }
        }

        public void Fill(T value)
        {
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = value;
            }
        }

        private bool IsNotWithinDimensions(int x, int y)
        {
            return x >= _dimensions.Width || x < 0 || y >= _dimensions.Height || y < 0;
        }

        public IEnumerable<T> ValuesOn(DiscreteAxisAlignedLine line)
        {
            var map = this;
            return 
            line
            .Slots()
            .Select(x => map[x]);
        }

        public override string ToString()
        {
            return "Map of " + typeof(T).Name;
        }

        public bool Equals(Map<T> other)
        {
            return _dimensions.Equals(other._dimensions) && _data.AllEquals(other._data);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_data).GetEnumerator();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}