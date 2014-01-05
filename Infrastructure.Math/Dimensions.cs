namespace Infrastructure.Math
{
    public struct Dimensions
    {
        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Dimensions(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int DimensionFor(Axis axis)
        {
            return axis == Axis.Horizontal ? _width : _height;
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

        private readonly int _width;
        private readonly int _height;
    }
}