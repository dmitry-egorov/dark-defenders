using System;

namespace Infrastructure.Util
{
    public static class DoubleExtensions
    {
        private const double DefaultTolerance = 0.00000000000002;

        public static int ToInt(this double d)
        {
            return (int) d;
        }

        public static float ToSingle(this double d)
        {
            return (float)d;
        }

        public static double TolerantFloor(this double d, double tolerance = DefaultTolerance)
        {
            var ceiling = Math.Ceiling(d);
            return ceiling - d < tolerance ? ceiling : Math.Floor(d);
        }

        public static double TolerantCeiling(this double d, double tolerance = DefaultTolerance)
        {
            var floor = Math.Floor(d);

            return d - floor < tolerance ? floor : Math.Ceiling(d);
        }

        public static double PrevInteger(this double d)
        {
            var floor = Math.Floor(d);

            return floor == d ? d - 1.0 : floor;
        }

        public static double NextInteger(this double d)
        {
            var ceiling = Math.Ceiling(d);

            return ceiling == d ? d + 1.0 : ceiling;
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
    }
}