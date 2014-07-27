using System.Collections.Generic;
using System.Drawing;

namespace Infrastructure.Math
{
    public struct DiscreteAxisAlignedLine
    {
        private readonly Axis _orientation;
        private readonly Point _origin;
        private readonly int _length;

        public DiscreteAxisAlignedLine(Axis orientation, Point origin, int length)
        {
            _length = length;
            _orientation = orientation;
            _origin = origin;
        }
        public IEnumerable<Point> Slots()
        {
            var start = _origin.CoordinateFor(_orientation);
            var level = _origin.CoordinateFor(_orientation.Other());

            for (var i = 0; i < _length; i++)
            {
                var current = start + i;
                yield return _orientation.PointAlong(current, level);
            }
        }

        public static DiscreteAxisAlignedLine From(Axis orientation, int start, int end, int level)
        {
            var origin = orientation.PointAlong(start, level);
            var length = start - end;

            return new DiscreteAxisAlignedLine(orientation, origin, length);
        }
    }
}