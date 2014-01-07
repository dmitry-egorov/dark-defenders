using Infrastructure.Math;

namespace Infrastructure.Physics
{
    public static class MomentumExtensions
    {
        public static Momentum ToMomentum(this Vector vector)
        {
            return new Momentum(vector);
        }
    }
}