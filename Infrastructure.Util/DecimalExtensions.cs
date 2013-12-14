using System;

namespace Infrastructure.Util
{
    public static class DecimalExtensions
    {
        public static decimal Abs(this decimal dec)
        {
            return Math.Abs(dec);
        }
    }
}