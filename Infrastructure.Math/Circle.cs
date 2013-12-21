using Infrastructure.Util;

namespace Infrastructure.Math
{
    public class Circle: ValueObject<Circle>
    {
        public Vector Position { get; private set; }
        public double Radius { get; private set; }

        public Circle(Vector position, double radius)
        {
            Position = position;
            Radius = radius;
        }

        public Circle ChangePosition(Vector newPosition)
        {
            return new Circle(newPosition, Radius);
        }

        public bool IsAboveHorizontalAxis()
        {
            return Position.Y - Radius > 0d;
        }

        protected override string ToStringInternal()
        {
            return "[{0}], {1}".FormatWith(Position, Radius);
        }

        protected override bool EqualsInternal(Circle other)
        {
            return Position.Equals(other.Position) && Radius.Equals(other.Radius);
        }

        protected override int GetHashCodeInternal()
        {
            unchecked
            {
                return (Position.GetHashCode() * 397) ^ Radius.GetHashCode();
            }
        }
    }
}