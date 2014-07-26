namespace Infrastructure.Math
{
    public static class Round
    {
        public static double RoundTo(this double value, double precision)
        {
            return System.Math.Round(precision*value) / precision;
        }
    }
}