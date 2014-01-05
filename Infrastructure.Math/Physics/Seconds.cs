using System.Globalization;

namespace Infrastructure.Math.Physics
{
    public struct Seconds
    {
        public static Seconds Zero = new Seconds();
        public double Value { get; private set; }

        public Seconds(double value) : this()
        {
            Value = value;
        }

        public bool Equals(Seconds other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Seconds && Equals((Seconds) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public static Seconds operator +(Seconds arg1, Seconds arg2)
        {
            return new Seconds(arg1.Value + arg2.Value);
        }

        public static Seconds operator -(Seconds arg1, Seconds arg2)
        {
            return new Seconds(arg1.Value - arg2.Value);
        }

        public static bool operator <(Seconds arg1, Seconds arg2)
        {
            return arg1.Value < arg2.Value;
        }

        public static bool operator >(Seconds arg1, Seconds arg2)
        {
            return arg1.Value > arg2.Value;
        }

        public static bool operator <=(Seconds arg1, Seconds arg2)
        {
            return arg1.Value <= arg2.Value;
        }

        public static bool operator >=(Seconds arg1, Seconds arg2)
        {
            return arg1.Value >= arg2.Value;
        }
    }
}