using System.Globalization;
using System.Threading;

namespace Infrastructure.DDDES
{
    public abstract class Identity
    {
        private static long _currentIdValue;

        private readonly long _value;

        public static void Reset()
        {
            Interlocked.Exchange(ref _currentIdValue, 0L);
        }

        protected Identity()
        {
            _value = Interlocked.Increment(ref _currentIdValue);
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return EqualsInternal((Identity) obj);
        }

        public bool Equals(Identity obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return EqualsInternal(obj);
        }

        public static bool operator ==(Identity arg1, Identity arg2)
        {
            if (ReferenceEquals(null, arg1)) return ReferenceEquals(null, arg2);
            if (ReferenceEquals(null, arg2)) return false;
            if (ReferenceEquals(arg1, arg2)) return true;
            return arg1.EqualsInternal(arg2);
        }

        public static bool operator !=(Identity arg1, Identity arg2)
        {
            return !(arg1 == arg2);
        }

        private bool EqualsInternal(Identity identity)
        {
            return _value.Equals(identity._value);
        }
    }
}