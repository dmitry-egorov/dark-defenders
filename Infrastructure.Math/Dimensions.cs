using System;

namespace Infrastructure.Math
{
    public struct Dimensions
    {
        public double Width
        {
            get { return _width; }
        }

        public double Height
        {
            get { return _height; }
        }

        public Dimensions(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public bool Equals(Dimensions other)
        {
            return _width.Equals(other._width) && _height.Equals(other._height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Dimensions && Equals((Dimensions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_width.GetHashCode()*397) ^ _height.GetHashCode();
            }
        }

        private readonly Double _width;
        private readonly Double _height;
    }
}