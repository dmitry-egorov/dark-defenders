using Infrastructure.Util;

namespace DarkDefenders.Game.Model.EntityProperties
{
    public class RigidBodyProperties : SlowValueObject
    {
        public double BoundingBoxRadius { get; private set; }
        public double Mass { get; private set; }
        public double HorizontalMomentumLimit { get; private set; }

        public RigidBodyProperties(double boundingBoxRadius, double mass, double horizontalMomentumLimit)
        {
            BoundingBoxRadius = boundingBoxRadius;
            Mass = mass;
            HorizontalMomentumLimit = horizontalMomentumLimit;
        }
    }
}