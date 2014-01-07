using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Map<T>: IEnumerable<T> 
        where T: struct 
    {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAnyAtLine(Axis axis, int mainStart, int mainEnd, int other, T value)
        {
            var mainDimension = _dimensions.DimensionFor(axis);
            var otherDimension = _dimensions.DimensionFor(axis.Other());

            if (other < 0 || other >= otherDimension)
            {
                return _defaultItem.Equals(value);
            }

            if (mainStart < 0)
            {
                if (_defaultItem.Equals(value))
                {
                    return true;
                }

                mainStart = 0;
            }

            if (mainEnd >= mainDimension)
            {
                if (_defaultItem.Equals(value))
                {
                    return true;
                }

                mainEnd = mainDimension - 1;
            }

            var width = _dimensions.Width;
            var start = axis == Axis.Horizontal ? other * width + mainStart : mainStart * width + other;
            var count = mainEnd - mainStart + 1;
            var step = axis == Axis.Horizontal ? 1 : width;

            for (int i = 0, index = start; i < count; i++, index += step)
            {
                var isSolid = _data[index].Equals(value);

                if (isSolid)
                {
                    return true;
                }
            }

            return false;
        }

        public void Fill(T value)
        {
            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = value;
            }
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

        private bool IsNotWithinDimensions(int x, int y)
        {
            return x >= _dimensions.Width || x < 0 || y >= _dimensions.Height || y < 0;
        }

        private readonly Dimensions _dimensions;
        private readonly T[] _data;
        private readonly T _defaultItem;
    }
}