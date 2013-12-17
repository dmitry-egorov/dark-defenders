using Infrastructure.Util;

namespace Infrastructure.Math
{
    public class Vector
    {
        public static readonly Vector Zero  = new Vector(0, 0);
        public static readonly Vector Left  = new Vector(-1, 0);
        public static readonly Vector Right = new Vector(1, 0);
        public static readonly Vector Down  = new Vector(0, -1);

        public static Vector XY(double x, double y)
        {
            return new Vector(x, y);
        }

        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
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
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Vector arg1, Vector arg2)
        {
            if (ReferenceEquals(null, arg1)) return ReferenceEquals(null, arg2);
            if (ReferenceEquals(arg1, arg2)) return true;
            return arg1.Equals(arg2);
        }

        public static bool operator !=(Vector arg1, Vector arg2)
        {
            return !(arg1 == arg2);
        }

        public static Vector operator *(Vector vector, double d)
        {
            return new Vector(vector.X * d, vector.Y * d);
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        public double LengthSquared()
        {
            return X*X + Y*Y;
        }
    }
}