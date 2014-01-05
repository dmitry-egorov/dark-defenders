using System.Runtime.CompilerServices;

namespace Infrastructure.Math.Physics
{
    public struct Momentum
    {
        public static Momentum Zero = Vector.Zero.ToMomentum();
        public static Momentum Left = Vector.Left.ToMomentum();
        public static Momentum Right = Vector.Right.ToMomentum();

        public Vector Value { get; private set; }

        public Momentum(Vector value)
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

        public bool Equals(Momentum other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Momentum && Equals((Momentum)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static Momentum operator +(Momentum arg1, Momentum arg2)
        {
            return new Momentum(arg1.Value + arg2.Value);
        }

        public static Momentum operator -(Momentum arg1, Momentum arg2)
        {
            return new Momentum(arg1.Value - arg2.Value);
        }

        public static Vector operator *(Momentum arg1, Seconds seconds)
        {
            return arg1.Value * seconds.Value;
        }
    }
}