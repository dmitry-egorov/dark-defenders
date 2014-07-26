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
        private readonly Dimensions _dimensions;
        private readonly T[] _data;
        private readonly T _defaultItem;

        public Dimensions Dimensions
        {
            get { return _dimensions; }
        }

        public T DefaultItem
        {
            get { return _defaultItem; }
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
                    return DefaultItem;
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
                return DefaultItem.Equals(value);
            }

            if (mainStart < 0)
            {
                if (DefaultItem.Equals(value))
                {
                    return true;
                }

                mainStart = 0;
            }

            if (mainEnd >= mainDimension)
            {
                if (DefaultItem.Equals(value))
                {
                    return true;
                }

                mainEnd = mainDimension - 1;
            }

            var width = _dimensions.Width;
            var start = axis == Axis.X ? other * width + mainStart : mainStart * width + other;
            var count = mainEnd - mainStart + 1;
            var step = axis == Axis.X ? 1 : width;

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

        public bool IsTouchingWallsAt(Axis axis, double mainCenter, double radius, int other, T blockValue)
        {
            var start = (mainCenter - radius).TolerantFloor().ToInt();
            var end = (mainCenter + radius).PrevInteger().ToInt();

            return IsAnyAtLine(axis, start, end, other, blockValue);
        }

        public Vector IntersectMovingBox(ObjectInSpace objectInSpace, Vector positionDelta, T blockValue)
        {
            var center = objectInSpace.GetPosition();
            var box = objectInSpace.GetBoundingBox();

            Vector horizontalAdjustment;
            var horizontalPositionAdjusted = ApplyAxisPositionChange(Axis.X, center, box, positionDelta, blockValue, out horizontalAdjustment);

            Vector verticalAdjustment;
            var verticalPositionAdjusted = ApplyAxisPositionChange(Axis.Y, center, box, positionDelta, blockValue, out verticalAdjustment);

            if (verticalPositionAdjusted && horizontalPositionAdjusted)
            {
                var horizontalDelta = (horizontalAdjustment - center).LengthSquared();
                var verticalDelta = (verticalAdjustment - center).LengthSquared();

                return
                    horizontalDelta < verticalDelta
                        ? horizontalAdjustment
                        : verticalAdjustment;
            }

            if (horizontalPositionAdjusted)
            {
                return horizontalAdjustment;
            }

            if (verticalPositionAdjusted)
            {
                return verticalAdjustment;
            }

            return center + positionDelta;
        }

        private bool ApplyAxisPositionChange(Axis axis, Vector center, Box boundingBox, Vector positionDelta, T blockValue, out Vector adjustedPosition)
        {
            var mainCenter = center.CoordinateFor(axis);
            var otherCenter = center.CoordinateFor(axis.Other());

            var mainRadius = boundingBox.RadiusFor(axis);
            var otherRadius = boundingBox.RadiusFor(axis.Other());

            var dMain = positionDelta.CoordinateFor(axis);
            var dOther = positionDelta.CoordinateFor(axis.Other());

            if (dMain == 0.0)
            {
                adjustedPosition = Vector.Zero;
                return false;
            }

            var sign = System.Math.Sign(dMain);
            var slope = dOther / dMain;

            var boundOffset = sign * mainRadius;
            var startBoundOther = otherCenter;
            var startBoundMain = mainCenter + boundOffset;
            var endBoundMain = startBoundMain + dMain;

            var start = ((sign == 1) ? startBoundMain.TolerantCeiling() : startBoundMain.TolerantFloor()).ToInt();
            var end = ((sign == 1) ? endBoundMain.PrevInteger() : endBoundMain.NextInteger()).ToInt();

            for (var main = start; (end - main) * sign >= 0; main += sign)
            {
                var mainToCheck = (sign == 1) ? main : main - 1;
                var other = slope * (main - startBoundMain) + startBoundOther;

                var isTouchingWalls = IsTouchingWallsAt(axis.Other(), other, otherRadius, mainToCheck, blockValue);

                if (!isTouchingWalls)
                {
                    continue;
                }

                adjustedPosition = Vector.ByAxis(axis, main - boundOffset, other);
                return true;
            }

            adjustedPosition = Vector.Zero;
            return false;
        }
    }
}