using System.Runtime.CompilerServices;

namespace Infrastructure.Math
{
    public static class AxisExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Axis Other(this Axis axis)
        {
            return axis == Axis.Horizontal ? Axis.Vertical : Axis.Horizontal;
        }
    }
}