using System.Runtime.CompilerServices;
using Infrastructure.Math;

namespace Infrastructure.Physics
{
    public struct Force
    {
        public static Force Zero = Vector.Zero.ToForce();
        public static Force Left = Vector.Left.ToForce();
        public static Force Right = Vector.Right.ToForce();

        public Vector Value { get; private set; }
        public Force(double x, double y): this (Vector.XY(x, y))
        {
        }
        public Force(Vector value)
            : this()
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NotEqualsZero()
        {
            return !EqualsZero();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualsZero()
        {
            return Value.EqualsZero();
        }

        public bool Equals(Force other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Force && Equals((Force) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static Force operator +(Force arg1, Force arg2)
        {
            return new Force(arg1.Value + arg2.Value);
        }

        public static Force operator -(Force arg1, Force arg2)
        {
            return new Force(arg1.Value - arg2.Value);
        }

        public static Force operator *(Force arg1, double coeffiecient)
        {
            return new Force(arg1.Value * coeffiecient);
        }

        public static Momentum operator *(Force arg1, Seconds coeffiecient)
        {
            return new Momentum(arg1.Value * coeffiecient.Value);
        }
    }
}