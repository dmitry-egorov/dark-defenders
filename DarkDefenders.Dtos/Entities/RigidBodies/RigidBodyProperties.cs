using Infrastructure.Util;

namespace DarkDefenders.Dtos.Entities.RigidBodies
{
    public class RigidBodyProperties : SlowValueObject<RigidBodyProperties>
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