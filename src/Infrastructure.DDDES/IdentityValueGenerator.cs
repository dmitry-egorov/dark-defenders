using System.Threading;

namespace Infrastructure.DDDES
{
    public static class IdentityValueGenerator
    {
        private static long _currentIdValue;

        public static void Reset()
        {
            Interlocked.Exchange(ref _currentIdValue, 0L);
        }

        public static long Generate()
        {
            return Interlocked.Increment(ref _currentIdValue);
        }
    }
}