using System.Runtime.CompilerServices;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Circle
    {
        private readonly Vector _position;
        private readonly double _radius;

        public Vector Position { get { return _position; } }

        public double Radius { get { return _radius; } }

        public Circle(Vector position, double radius)
        {
            _position = position;
            _radius = radius;
        }

        public Circle ChangePosition(Vector newPosition)
        {
            return new Circle(newPosition, Radius);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsAboveHorizontalAxis()
        {
            return _position.Y - _radius > 0d;
        }

        public override string ToString()
        {
            return "[{0}], {1}".FormatWith(_position, _radius);
        }

        public bool Equals(Circle other)
        {
            return _position.Equals(other._position) && _radius.Equals(other._radius);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Circle && Equals((Circle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_position.GetHashCode()*397) ^ _radius.GetHashCode();
            }
        }
    }
}