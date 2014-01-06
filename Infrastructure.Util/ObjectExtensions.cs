namespace Infrastructure.Util
{
    public static class ObjectExtensions
    {
        public static bool IsNot<T>(this object obj)
        {
            return !(obj is T);
        }
    }
}