using Infrastructure.Util;

namespace DarkDefenders.Game.Model.EntityProperties
{
    public class RigidBodyProperties : SlowValueObject
    {
        public double BoundingBoxRadius { get; private set; }
        public double Mass { get; private set; }
        public double TopHorizontalMomentum { get; private set; }

        public RigidBodyProperties(double boundingBoxRadius, double mass, double topHorizontalMomentum)
        {
            BoundingBoxRadius = boundingBoxRadius;
            Mass = mass;
            TopHorizontalMomentum = topHorizontalMomentum;
        }
    }
}