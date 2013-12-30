using System.Runtime.CompilerServices;
using Infrastructure.Util;

namespace Infrastructure.Math
{
    public struct Vector
    {
        public static readonly Vector Zero  = new Vector(0, 0);
        public static readonly Vector Left  = new Vector(-1, 0);
        public static readonly Vector Right = new Vector(1, 0);
        public static readonly Vector Down  = new Vector(0, -1);
        
        private readonly double _x;
        private readonly double _y;

        public static Vector XY(double x, double y)
        {
            return new Vector(x, y);
        }

        public double X { get { return _x; } }

        public double Y { get { return _y; } }

        public Vector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEqualsZero()
        {
            return !EqualsZero();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualsZero()
        {
            return _x.Equals(0.0) && _y.Equals(0.0);
        }

        public double LengthSquared()
        {
            return _x*_x + _y*_y;
        }

        public override string ToString()
        {
            return "({0}, {1})".FormatWith(X, Y);
        }

        public string ToString(string formatting)
        {
            return "({0}, {1})".FormatWith(X.ToString(formatting), Y.ToString(formatting));
        }

        public bool Equals(Vector other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector && Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_x.GetHashCode()*397) ^ _y.GetHashCode();
            }
        }


        public static bool operator ==(Vector arg1, Vector arg2)
        {
            return arg1.Equals(arg2);
        }

        public static bool operator !=(Vector arg1, Vector arg2)
        {
            return !(arg1 == arg2);
        }

        public static Vector operator *(Vector vector, double d)
        {
            return new Vector(vector._x * d, vector._y * d);
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1._x - vector2._x, vector1._y - vector2._y);
        }
    }
}