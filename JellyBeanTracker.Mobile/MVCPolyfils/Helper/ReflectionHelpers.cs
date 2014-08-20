using System;
using System.Reflection;
using System.Linq;

namespace MVCPolyfils.Helper
{
    public class ReflectionHelpers
    {
        public ReflectionHelpers ()
        {
        }

        public static PropertyInfo GetProperty(Type sourceType, string propertyName)
        {
            var allProperties = sourceType.GetRuntimeProperties ();
            var property = allProperties.Where(
                mi => string.Equals(propertyName, mi.Name, StringComparison.Ordinal)).ToList();

            if (property.Count > 1)
            {
                throw new AmbiguousMatchException();
            }

            return property.FirstOrDefault();
        }

        public static MethodInfo GetMethod(Type sourceType, string methodName)
        {
            var allMethods = sourceType.GetRuntimeMethods ();
            var methods = allMethods.Where(
                mi => string.Equals(methodName, mi.Name, StringComparison.Ordinal)).ToList();

            if (methods.Count > 1)
            {
                throw new AmbiguousMatchException();
            }

            return methods.FirstOrDefault();
        }
    }
}

