namespace DarkDefenders.Domain.Players.Entities.Projectiles
{
    public class Projectile
    {
        public const double Momentum = 3.0 * Mass;
        public const double Mass = 0.001;
        public const double BoundingCircleRadius = 1.0 / 100.0;
    }
}