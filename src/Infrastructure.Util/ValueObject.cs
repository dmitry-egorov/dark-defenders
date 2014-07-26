namespace Infrastructure.Util
{
    public abstract class ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((T) obj);
        }

        public bool Equals(T other)
        {
            return EqualsInternal(other);
        }

        public override int GetHashCode()
        {
            return GetHashCodeInternal();
        }

        public override string ToString()
        {
            return ToStringInternal();
        }

        protected abstract string ToStringInternal();
        protected abstract bool EqualsInternal(T other);
        protected abstract int GetHashCodeInternal();
    }
}