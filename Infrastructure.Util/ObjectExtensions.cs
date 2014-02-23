using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Util
{
    public static class ObjectExtensions
    {

        public static bool NullSafe<T>(this T obj, Action<T> action)
            where T: class 
        {
            if (obj == null)
            {
                return false;
            }

            action(obj);
            return true;
        }

        public static bool IsIn<T>(this T obj, params T[] objects) 
        {
            return objects.Any(o => obj.Equals(o));
        }

        public static bool IsNot<T>(this object obj)
        {
            return !(obj is T);
        }

        public static string SafeToString(this object o)
        {
            if (o == null)
            {
                return "null";
            }

            return o.ToString();
        }

        public static int SafeGetHashCode(this object o)
        {
            if (o == null)
            {
                return 0;
            }

            return o.GetHashCode();
        }

        public static bool SafeEquals(this object o1, object o2)
        {
            if (o1 == null)
            {
                if (o2 != null)
                {
                    return false;
                }
            }
            else if (!o1.Equals(o2))
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<FieldInfo> GetAllInstanceFields(this Type t)
        {
            if (t == null)
                return Enumerable.Empty<FieldInfo>();

            
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                       BindingFlags.Instance | BindingFlags.DeclaredOnly;
            return t.GetFields(flags).Concat(GetAllInstanceFields(t.BaseType));
        }
    }
}