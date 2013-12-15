using System;

namespace Infrastructure.DDDES
{
    public abstract class Identity
    {
        private readonly Guid _value;

        protected Identity(Guid value)
        {
            _value = value;
        }

        protected Identity()
        {
            _value = Guid.NewGuid();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
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