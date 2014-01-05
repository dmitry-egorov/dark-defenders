using Infrastructure.Util;

namespace DarkDefenders.Domain.RigidBodies
{
    public class RigidBodyProperties : ValueObject<RigidBodyProperties>
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

        protected override string ToStringInternal()
        {
            return "Radius: {0}, mass: {1}, top horizontal momentum: {2}"
                   .FormatWith(BoundingBoxRadius, Mass, TopHorizontalMomentum);
        }

        protected override bool EqualsInternal(RigidBodyProperties other)
        {
            return BoundingBoxRadius.Equals(other.BoundingBoxRadius)
                && Mass.Equals(other.Mass)
                && TopHorizontalMomentum.Equals(other.TopHorizontalMomentum);
        }

        protected override int GetHashCodeInternal()
        {
            var hashCode = BoundingBoxRadius.GetHashCode();
            hashCode = (hashCode * 397) ^ Mass.GetHashCode();
            hashCode = (hashCode * 397) ^ TopHorizontalMomentum.GetHashCode();
            return hashCode;
        }
    }
}