using System.Globalization;

namespace Infrastructure.DDDES
{
    public class IdentityOf<TEntity>
    {
        private readonly long _value;

        public IdentityOf(long value)
        {
            _value = value;
        }

        public IdentityOf()
        {
            _value = IdentityValueGenerator.Generate();
        }

        public static bool operator ==(IdentityOf<TEntity> arg1, IdentityOf<TEntity> arg2)
        {
            if (ReferenceEquals(null, arg1)) return ReferenceEquals(null, arg2);
            if (ReferenceEquals(null, arg2)) return false;
            if (ReferenceEquals(arg1, arg2)) return true;
            return arg1.EqualsInternal(arg2);
        }

        public static bool operator !=(IdentityOf<TEntity> arg1, IdentityOf<TEntity> arg2)
        {
            return !(arg1 == arg2);
        }

        public long Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return EqualsInternal((IdentityOf<TEntity>)obj);
        }

        public bool Equals(IdentityOf<TEntity> obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return EqualsInternal(obj);
        }

        private bool EqualsInternal(IdentityOf<TEntity> identity)
        {
            return Value.Equals(identity.Value);
        }
    }
}