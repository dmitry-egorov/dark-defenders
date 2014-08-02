using System;

namespace Infrastructure.Math
{
    public static class DoubleExtensions
    {
        private const double DefaultTolerance = 0.00000000000002;

        public static int ToInt(this double d)
        {
            return (int)d;
        }

        public static float ToSingle(this double d)
        {
            return (float)d;
        }

        public static double Floor(this double d)
        {
            return System.Math.Floor(d);
        }

        public static double Ceiling(this double d)
        {
            return System.Math.Ceiling(d);
        }

        public static int ToIntTolerant(this double d, double tolerance = DefaultTolerance)
        {
            return d.TolerantFloor(tolerance).ToInt();
        }

        public static double TolerantFloor(this double d, double tolerance = DefaultTolerance)
        {
            var ceiling = System.Math.Ceiling(d);

            return ceiling - d <= tolerance ? ceiling : d.Floor();
        }

        public static double LimitTop(this double d, double top)
        {
            return d > top ? top : d;
        }

        public static double LimitBottom(this double d, double bottom)
        {
            return d < bottom ? bottom : d;
        }

        public static double Limit(this double d, double top, double bottom)
        {
            if (d > top)
            {
                return top;
            }

            if (d < bottom)
            {
                return bottom;
            }

            return d;
        }

        public static int ToIntTolerantIn(this double value, AxisDirection direction, double tolerance = DefaultTolerance)
        {
            switch (direction)
            {
                case AxisDirection.Positive:
                    return value.PrevIntTolerant(tolerance);
                case AxisDirection.Negative:
                    return value.ToIntTolerant(tolerance);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        private static int PrevIntTolerant(this double value, double tolerance)
        {
            var floor = value.Floor();
            return value - floor <= tolerance ? floor.ToInt() - 1 : floor.ToInt();
        }

        public static AxisDirection Direction(this double d)
        {
            if (d > 0)
            {
                return AxisDirection.Positive;
            }

            if (d < 0)
            {
                return AxisDirection.Negative;
            }

            return AxisDirection.None;
        }

        public static double RoundTo(this double d, AxisDirection axisDirection)
        {
            switch (axisDirection)
            {
                case AxisDirection.Positive:
                    return d.Ceiling();
                case AxisDirection.Negative:
                    return d.Floor();
                default:
                    throw new ArgumentOutOfRangeException("axisDirection");
            }
        }

        public static double Abs(this double d)
        {
            return System.Math.Abs(d);
        }
    }
}