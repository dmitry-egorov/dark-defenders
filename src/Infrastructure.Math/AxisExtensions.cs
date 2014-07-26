using System.Runtime.CompilerServices;

namespace Infrastructure.Math
{
    public static class AxisExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Axis Other(this Axis axis)
        {
            return axis == Axis.X ? Axis.Y : Axis.X;
        }
    }
}