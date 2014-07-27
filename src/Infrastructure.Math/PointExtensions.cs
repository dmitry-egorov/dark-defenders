using System.Drawing;

namespace Infrastructure.Math
{
    public static class PointExtensions
    {
        public static int CoordinateFor(this Point point, Axis axis)
        {
            return axis == Axis.X ? point.X : point.Y;
        }

        public static Point PointAlong(this Axis axis, int current, int level)
        {
            return axis == Axis.X ? new Point(current, level) : new Point(level, current);
        }
    }
}