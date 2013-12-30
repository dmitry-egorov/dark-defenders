using System;

namespace Infrastructure.Util
{
    public static class DoubleExtensions
    {
        public static int ToInt(this double d)
        {
            return (int) d;
        }

        public static double Floor(this double d)
        {
            return Math.Floor(d);
        }

        public static double Ceiling(this double d)
        {
            return Math.Ceiling(d);
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