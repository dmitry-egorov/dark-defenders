namespace Infrastructure.Math.Physics
{
    public static class MomentumExtensions
    {
        public static Momentum ToMomentum(this Vector vector)
        {
            return new Momentum(vector);
        }
    }
}