using Infrastructure.Util;

namespace DarkDefenders.Domain.Model.Entities.RigidBodies
{
    public class RigidBodyProperties : SlowValueObject
    {
        public float BoundingBoxRadius { get; private set; }
        public float Mass { get; private set; }
        public float TopHorizontalMomentum { get; private set; }

        public RigidBodyProperties(float boundingBoxRadius, float mass, float topHorizontalMomentum)
        {
            BoundingBoxRadius = boundingBoxRadius;
            Mass = mass;
            TopHorizontalMomentum = topHorizontalMomentum;
        }
    }
}