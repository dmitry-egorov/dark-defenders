using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Util
{
    public abstract class SlowValueObject : ValueObject<SlowValueObject>
    {
        protected override string ToStringInternal()
        {
            var name = GetType().Name;
            var fields = GetFields();
            var values = fields
                .Select(x => x.GetValue(this))
                .Select(GetStringOf)
                .JoinBy(", ");

            return "{0}: {1}".FormatWith(name, values);
        }

        private static string GetStringOf(object o)
        {
            if (o == null)
            {
                return "";
            }

            var enumerable = o as IEnumerable;

            if (enumerable != null)
            {
                return "[" + enumerable.Cast<object>().Select(GetStringOf).JoinBy(", ") + "]";
            }

            return o.ToString();
        }

        protected override int GetHashCodeInternal()
        {
            var values = GetFields().Select(x => x.GetValue(this));

            return values.Aggregate(0, (hash, item) => (hash * 397) ^ GetHashCodeOf(item));
        }

        protected override bool EqualsInternal(SlowValueObject other)
        {
            var fields = GetFields().AsReadOnly();

            var values1 = fields.Select(x => x.GetValue(this));
            var values2 = fields.Select(x => x.GetValue(other));

            return values1.Zip(values2, AreEqual).All(x => x);
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            return GetType().GetAllInstanceFields();
        }

        private static int GetHashCodeOf(object o)
        {
            if (o == null)
            {
                return 0;
            }

            var enumerable = o as IEnumerable;

            if (enumerable != null)
            {
                return enumerable.Cast<object>().AllHashCode();
            }

            return o.GetHashCode();
        }

        private static bool AreEqual(object o1, object o2)
        {
            if (o1 == null)
            {
                return o2 == null;
            }

            var enumeration1 = o1 as IEnumerable;
            var enumeration2 = o2 as IEnumerable;

            if (enumeration1 != null)
            {
                if (enumeration2 == null)
                {
                    return false;
                }

                return enumeration1.Cast<object>().AllEquals(enumeration2.Cast<object>());
            }

            return o1.Equals(o2);
        }
    }
}