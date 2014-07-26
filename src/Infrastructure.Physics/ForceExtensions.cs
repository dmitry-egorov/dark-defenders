using Infrastructure.Math;

namespace Infrastructure.Physics
{
    public static class ForceExtensions
    {
        public static Force ToForce(this Vector vector)
        {
            return new Force(vector);
        }
    }
}