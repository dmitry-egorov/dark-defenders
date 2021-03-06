﻿using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Bounds
    {
        private readonly double _widthRadius;
        private readonly double _heightRadius;

        public double WidthRadius { get { return _widthRadius; } }
        public double HeightRadius { get { return _heightRadius; } }

        public static Bounds Square(double radius)
        {
            return new Bounds(radius, radius);
        }

        public Bounds(double widthRadius, double heightRadius)
        {
            _widthRadius = widthRadius;
            _heightRadius = heightRadius;
        }

        public double RadiusFor(Axis axis)
        {
            return axis == Axis.X ? _widthRadius : _heightRadius;
        }

        public override string ToString()
        {
            return "{0}, {1}".FormatWith(_widthRadius, _heightRadius);
        }

        public bool Equals(Bounds other)
        {
            return _widthRadius.Equals(other._widthRadius)
                && _heightRadius.Equals(other._heightRadius);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Bounds && Equals((Bounds) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _widthRadius.GetHashCode();
                hashCode = (hashCode*397) ^ _heightRadius.GetHashCode();
                return hashCode;
            }
        }
    }
}