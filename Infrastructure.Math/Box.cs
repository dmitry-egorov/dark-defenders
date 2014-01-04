using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Box
    {
        private readonly Vector _center;
        private readonly double _widthRadius;
        private readonly double _heightRadius;

        public Vector Center { get { return _center; } }

        public double WidthRadius { get { return _widthRadius; } }
        public double HeightRadius { get { return _heightRadius; } }

        public Box(Vector center, double widthRadius, double heightRadius)
        {
            _center = center;
            _widthRadius = widthRadius;
            _heightRadius = heightRadius;
        }

        public double RadiusFor(Axis axis)
        {
            return axis == Axis.Horizontal ? _widthRadius : _heightRadius;
        }

        public Box ChangePosition(Vector newPosition)
        {
            return new Box(newPosition, _widthRadius, _heightRadius);
        }

        public override string ToString()
        {
            return "[{0}], {1}".FormatWith(_center, _widthRadius);
        }

        public bool Equals(Box other)
        {
            return _center.Equals(other._center) 
                && _widthRadius.Equals(other._widthRadius)
                && _heightRadius.Equals(other._heightRadius);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Box && Equals((Box) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _center.GetHashCode();
                hashCode = (hashCode*397) ^ _widthRadius.GetHashCode();
                hashCode = (hashCode*397) ^ _heightRadius.GetHashCode();
                return hashCode;
            }
        }
    }
}